using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using LetterAmazer.Business.Services.Domain.AddressInfos;
using LetterAmazer.Business.Services.Domain.Countries;
using LetterAmazer.Business.Services.Domain.Coupons;
using LetterAmazer.Business.Services.Domain.Customers;
using LetterAmazer.Business.Services.Domain.Invoice;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.Mails;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Utils.Helpers;
using LetterAmazer.Websites.Client.Attributes;
using LetterAmazer.Websites.Client.ViewModels;
using LetterAmazer.Websites.Client.Extensions;
using LetterAmazer.Websites.Client.ViewModels.Shared.Utils;
using log4net;
using System;
using System.Web.Mvc;
using ProductType = LetterAmazer.Business.Services.Domain.Products.ProductType;
using LetterAmazer.Websites.Client.ViewModels.User;
using LetterAmazer.Business.Services.Domain.Organisation;

namespace LetterAmazer.Websites.Client.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(UserController));
        private IOrderService orderService;
        private IPaymentService paymentService;
        private ILetterService letterService;
        private ICouponService couponService;
        private ICountryService countryService;
        private IPriceService priceService;
        private IOrganisationService organisationService;
        private IMailService mailService;
        private IInvoiceService invoiceService;
        private ICustomerService customerService;

        public UserController(IOrderService orderService, IPaymentService paymentService,
            ILetterService letterService, ICouponService couponService, ICountryService countryService,
            IPriceService priceService,
            IOrganisationService organisationService, IMailService mailService, IInvoiceService invoiceService,
            ICustomerService customerService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
            this.letterService = letterService;
            this.couponService = couponService;
            this.countryService = countryService;
            this.priceService = priceService;
            this.organisationService = organisationService;
            this.mailService = mailService;
            this.invoiceService = invoiceService;
            this.customerService = customerService;
        }

        public ActionResult Index(int? page, ProfileViewModel model)
        {
            buildOverviewModel(model);

            return View(model);
        }

        private void buildOverviewModel(ProfileViewModel model)
        {
            var orders = orderService.GetOrderBySpecification(new OrderSpecification()
            {
                UserId = SessionHelper.Customer.Id,
                FromDate = model.FromDate,
                ToDate = model.ToDate
            }).OrderByDescending(c => c.DateCreated);

            model.Orders = getOrderViewModel(orders);
            model.Customer = SessionHelper.Customer;
        }

        #region Send letter

        [HttpGet]
        public ActionResult SendALetter()
        {
            CreateSingleLetterModel model = new CreateSingleLetterModel();
           model.Email = SessionHelper.Customer.Email;

            if (SessionHelper.Customer.CreditLimit < SessionHelper.Customer.Credit)
            {
                model.HasCredits = true;
            }
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SendALetter(CreateSingleLetterModel model)
        {
            try
            {
                ValidateInput();

                Order order = new Order();

                order.Customer = SessionHelper.Customer;
                AddressInfo addressInfo = new AddressInfo();
                addressInfo.Address1 = model.DestinationAddress;
                addressInfo.FirstName = model.RecipientName;
                addressInfo.City = model.DestinationCity;
                addressInfo.Country = countryService.GetCountryBySpecificaiton(
                    new CountrySpecification() { CountryCode = model.DestinationCountryCode }).FirstOrDefault();
                addressInfo.Zipcode = model.ZipCode;


                LetterDetails letterDetail = new LetterDetails()
                {
                    LetterColor = LetterColor.Color,
                    LetterPaperWeight = LetterPaperWeight.Eight,
                    LetterProcessing = LetterProcessing.Dull,
                    LetterSize = LetterSize.A4,
                    LetterType = LetterType.Pres
                };

                Letter letter = new Letter()
                {
                    LetterDetails = letterDetail,
                    ToAddress = addressInfo,
                    LetterStatus = LetterStatus.Created,

                };

                if (model.UseUploadFile)
                {
                    logger.DebugFormat("upload file key: {0}", model.UploadFile);
                    letter.LetterContent.Path = model.UploadFile;
                }
                else
                {
                    string tempKeyName = string.Format("{0}/{1}/{2}.pdf", DateTime.Now.Year, DateTime.Now.Month,
                        Guid.NewGuid().ToString());
                    string tempPath = PathHelper.GetAbsoluteFile(tempKeyName);

                    var convertedText = HelperMethods.Utf8FixString(model.WriteContent);
                    PdfHelper.ConvertToPdf(tempPath, convertedText);
                    letter.LetterContent.Path = tempKeyName;
                    letter.LetterContent.WrittenContent = model.WriteContent;
                }
                if (System.IO.File.Exists(PathHelper.GetAbsoluteFile(letter.LetterContent.Path)))
                {
                    letter.LetterContent.Content =
                        System.IO.File.ReadAllBytes(PathHelper.GetAbsoluteFile(letter.LetterContent.Path));
                }

                var price = priceService.GetPriceByAddress(addressInfo, letter.LetterContent.PageCount);
                letter.OfficeProductId = price.OfficeProductId;

                Coupon coupon = null;
                if (!string.IsNullOrEmpty(model.VoucherCode))
                {
                    var voucher = couponService.GetCouponBySpecification(new CouponSpecification()
                    {
                        Code = model.VoucherCode
                    });
                    if (voucher != null && voucher.Any())
                    {
                        coupon = (Coupon)voucher.FirstOrDefault();
                    }
                }


                order.OrderLines.Add(new OrderLine()
                {
                    ProductType = ProductType.Letter,
                    BaseProduct = letter,
                    Price = new Price()
                    {
                        PriceExVat = priceService.GetPriceByLetter(letter).Total
                    }
                });

                var rest = addCouponlines(price, coupon, order);

                if (rest > 0)
                {
                    order.OrderLines.Add(new OrderLine()
                    {
                        ProductType = ProductType.Payment,
                        PaymentMethodId = 2, // Paypal
                        Price = new Price()
                        {
                            PriceExVat = rest
                        }
                    });
                }

                var storedOrder = orderService.Create(order);

                string redirectUrl = paymentService.Process(storedOrder);

                if (string.IsNullOrEmpty(redirectUrl))
                {
                    ProfileViewModel profileViewModel = new ProfileViewModel();
                    return RedirectToAction("Index", "User", profileViewModel);
                }

                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                logger.Error(ex.InnerException);
                ModelState.AddModelError("Business", ex.Message);
            }

            return RedirectToActionWithError("Index", model);
        }


        #endregion

        #region Organisation

        public ActionResult EditOrganisation()
        {
            var organisationId = SessionHelper.Customer.Organisation.Id;
            var organisation = organisationService.GetOrganisationById(organisationId);

            var organisationViewModel = new EditOrganisationViewModel();

            return View(organisationViewModel);
        }

        public ActionResult CreateOrganisation()
        {
            var orgView = new CreateOrganisationViewModel();

            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString()
                };
                orgView.Countries.Add(selectedItem);
            }

            return View(orgView);
        }

        [HttpPost]
        public ActionResult CreateOrganisation(CreateOrganisationViewModel model)
        {
            var organisation = new Organisation();
            organisation.Name = model.OrganisationName;
            organisation.Address.Address1 = model.Address1;
            organisation.Address.Address2 = model.Address2;
            organisation.Address.City = model.City;
            organisation.Address.Zipcode = model.ZipCode;
            organisation.Address.State = model.State;
            organisation.Address.Country = countryService.GetCountryById(int.Parse(model.SelectedCountry));

            var stored_organisation = organisationService.Create(organisation);

            var customer = customerService.GetCustomerById(SessionHelper.Customer.Id);
            customer.Organisation = stored_organisation;
            customer.OrganisationRole = OrganisationRole.Administrator;
            var updated_customer = customerService.Update(customer);


            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(customer.Id.ToString(),true);

            var profile_model = new ProfileViewModel();
            buildOverviewModel(profile_model);
            return View("Index", profile_model);
        }


        public ActionResult EditOrganisationSettings()
        {
            var editViewModel = new EditOrganisationSettingsViewModel();

            editViewModel.LetterTypes = ControllerHelpers.GetEnumSelectList<LetterType>();

            return View(editViewModel);
        }

        [HttpPost]
        public ActionResult EditOrganisationSettings(EditOrganisationSettingsViewModel organisationSettings)
        {
            var organisation = organisationService.GetOrganisationById(organisationSettings.OrganisationId);

            organisation.OrganisationSettings.PreferedCountryId = int.Parse(organisationSettings.PreferedCountry);
            organisation.OrganisationSettings.LetterType = (LetterType)organisationSettings.LetterType;
            
            organisationService.Update(organisation);

            return View(organisationSettings);
        }

        public ActionResult EditContacts()
        {
            return View(new EditContactsViewModel());
        }

        [HttpPost]
        public ActionResult EditContacts(EditContactsViewModel editContacts)
        {
           
            return View();
        }

        public ActionResult EditSingleContact(int organisationContactId)
        {
            var addressList = organisationService.GetAddressListById(organisationContactId);
            return View(new ContactViewModel());
        }

        #endregion

     
        [HttpGet, AutoErrorRecovery]
        public ActionResult Delete(int id)
        {
            Order order = orderService.GetOrderById(id);
            OrderDetailViewModel model = new OrderDetailViewModel();
            // model.Order = order;
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, string submitAction)
        {
            try
            {
                var delete_order = orderService.GetOrderById(id);
                orderService.Delete(delete_order);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                ModelState.AddBusinessError(ex.Message);
            }

            Order order = orderService.GetOrderById(id);
            OrderDetailViewModel model = new OrderDetailViewModel();
            // model.Order = order;
            return RedirectToActionWithError("Delete", model, new { id = id });
        }

        public ActionResult Details(int id)
        {
            Order order = orderService.GetOrderById(id);
            var model = getOrderDetailViewModel(order);
            return View(model);
        }

        public FileResult Download(int id)
        {
            Letter letter = letterService.GetLetterById(id);
            return File(letter.LetterContent.Content, "application/pdf", id + ".pdf");
        }

        [HttpGet]
        public ActionResult Credits()
        {
            CreditsViewModel model = new CreditsViewModel();
            model.Credit = SessionHelper.Customer.Credit;
            model.CreditLimit = SessionHelper.Customer.CreditLimit;

            var possiblePaymentMethods =
                paymentService.GetPaymentMethodsBySpecification(new PaymentMethodSpecification()
                {
                    CustomerId = SessionHelper.Customer.Id
                });

            model.PaymentMethods = getPaymentmethodViewModels(possiblePaymentMethods);

            return View(model);
        }




        [HttpPost]
        public ActionResult Credits(CreditsViewModel model)
        {
            var selectedPaymentMethod = paymentService.GetPaymentMethodById(model.SelectedPaymentMethod);
            var credit = new Credit();

            var creditLine = new OrderLine()
            {
                Quantity = model.PurchaseAmount,
                ProductType = ProductType.Credit,
                BaseProduct = credit,
                Price = new Price()
                {
                    PriceExVat = model.PurchaseAmount
                }
            };

            var paymentLine = new OrderLine()
            {
                PaymentMethodId = selectedPaymentMethod.Id,
                ProductType = ProductType.Payment,
                Price = new Price()
                {
                    PriceExVat = model.PurchaseAmount
                }
            };

            Order order = new Order
            {
                Price = new Price()
                {
                    PriceExVat = model.PurchaseAmount
                },
                Customer = SessionHelper.Customer
            };
            order.OrderLines.Add(creditLine);
            order.OrderLines.Add(paymentLine);

            var placed_order = orderService.Create(order);
            string redirectUrl = paymentService.Process(placed_order);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                return RedirectToAction("Confirmation", "SingleLetter");
            }
            return Redirect(redirectUrl);
        }


        [HttpGet]
        public ActionResult ResendConfirmationEmail()
        {
            mailService.ConfirmUser(SessionHelper.Customer);
            return Json("OK");
        }


        [HttpGet]
        public ActionResult InvoiceOverview()
        {
            var customer = SessionHelper.Customer;

            InvoiceOverviewViewModel invoiceOverview = new InvoiceOverviewViewModel();
            invoiceOverview.DateFrom = DateTime.Now.AddDays(-180).Date;
            invoiceOverview.DateFrom = DateTime.Now.Date;
            invoiceOverview.InvoiceSnippets = getInvoiceSnippets(invoiceOverview.DateFrom,
                invoiceOverview.DateTo,
                customer.Organisation.Id);

            return View(invoiceOverview);
        }

        #region Private helpers

        private OrderDetailViewModel getOrderDetailViewModel(Order order)
        {
            var letter = (Letter)order.OrderLines.FirstOrDefault(c => c.ProductType == ProductType.Letter).BaseProduct;


            OrderDetailViewModel viewModel = new OrderDetailViewModel()
            {
                AddressInfo = letter.ToAddress,
                DateCreated = order.DateCreated,
                DateModified = order.DateModified.HasValue ? order.DateModified.Value : order.DateCreated,
                DatePaid = order.DatePaid.HasValue ? order.DatePaid.Value : (DateTime?)null,
                DateSent = order.DateSent.HasValue ? order.DateSent.Value : (DateTime?)null,
                OrderStatus = order.OrderStatus,
                LetterDetails = letter.LetterDetails,
                Id = letter.Id

            };

            return viewModel;
        }

        private List<OrderViewModel> getOrderViewModel(IEnumerable<Order> orders)
        {
            List<OrderViewModel> ordersViewModels = new List<OrderViewModel>();
            foreach (var order in orders)
            {
                var letterLine = order.OrderLines.FirstOrDefault(c => c.ProductType == ProductType.Letter);

                // only if the line is a letter - it might be something like credits
                if (letterLine != null)
                {
                    var letter = (Letter)letterLine.BaseProduct;
                    OrderViewModel viewModel = new OrderViewModel()
                    {
                        OrderLines = getOrderLineViewModel(order.OrderLines),
                        DateCreated = order.DateCreated,
                        OrderStatus = order.OrderStatus,
                        Id = order.Id,
                        Price = order.Price.PriceExVat,
                        LetterStatus = letter.LetterStatus
                    };

                    ordersViewModels.Add(viewModel);
                }

            }
            return ordersViewModels;
        }

        private List<OrderLineViewModel> getOrderLineViewModel(IEnumerable<OrderLine> orderLines)
        {
            List<OrderLineViewModel> lines = new List<OrderLineViewModel>();
            foreach (var orderline in orderLines.Where(c => c.ProductType == ProductType.Letter))
            {
                lines.Add(new OrderLineViewModel()
                {
                    Quantity = orderline.Quantity,
                    OrderLineProductViewModel = getOrderLineProductViewModel((Letter)orderline.BaseProduct)
                });
            }
            return lines;
        }

        private OrderLineProductViewModel getOrderLineProductViewModel(Letter letter)
        {
            return new OrderLineProductViewModel()
            {

            };
        }

        private IEnumerable<PaymentMethodViewModel> getPaymentmethodViewModels(IEnumerable<PaymentMethods> paymentMethods)
        {
            return paymentMethods.Select(paymentMethodse => new PaymentMethodViewModel()
            {
                Id = paymentMethodse.Id,
                Name = paymentMethodse.Name
            }).ToList();
        }

        private decimal addCouponlines(Price price, Coupon coupon, Order order)
        {
            decimal rest = price.Total;
            if (coupon != null)
            {
                decimal chargeCoupon = 0.0m;
                if (rest > coupon.CouponValueLeft)
                {
                    chargeCoupon = coupon.CouponValueLeft;
                }
                else
                {
                    chargeCoupon = rest;
                }

                order.OrderLines.Add(new OrderLine()
                {
                    ProductType = ProductType.Payment,
                    PaymentMethodId = 3, // coupon                        
                    CouponId = coupon.Id,
                    Price = new Price()
                    {
                        PriceExVat = chargeCoupon
                    }
                });

                rest -= coupon.CouponValueLeft;
            }
            return rest;
        }

        private List<InvoiceSnippetViewModel> getInvoiceSnippets(DateTime from, DateTime to, int organisationId)
        {
            var invoices = invoiceService.GetInvoiceBySpecification(new InvoiceSpecification()
            {
                DateFrom = from,
                DateTo = to,
                OrganisationId = organisationId
            });
            var models = new List<InvoiceSnippetViewModel>();

            foreach (var invoice in invoices)
            {
                models.Add(new InvoiceSnippetViewModel()
                {
                    DateCreated = invoice.DateCreated,
                    OrderNumber = invoice.InvoiceNumber,
                    TotalPrice = invoice.PriceTotal,
                    InvoiceGuid = invoice.Guid
                });
            }


            return models;
        }

        #endregion
    }
}
