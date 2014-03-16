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
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;
using LetterAmazer.Business.Services.Domain.Pricing;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Domain.Products.ProductDetails;
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
        private IOfficeService officeService;
        private IOfficeProductService officeProductService;

        public UserController(IOrderService orderService, IPaymentService paymentService,
            ILetterService letterService, ICouponService couponService, ICountryService countryService,
            IPriceService priceService,
            IOrganisationService organisationService, IMailService mailService, IInvoiceService invoiceService,
            ICustomerService customerService,IOfficeService officeService, IOfficeProductService officeProductService)
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
            this.officeProductService = officeProductService;
            this.officeService = officeService;
        }

        public ActionResult Index(int? page, DashboardViewModel model)
        {
            buildOverviewModel(model);

            return View(model);
        }

        #region Send letter windowed

        public ActionResult SendWindowedLetter()
        {
            var windowedModel = new SendWindowedLetterViewModel();

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

                if (country.Id == 59)
                {
                    selectedItem.Selected = true;
                }

                windowedModel.Countries.Add(selectedItem);
            }

            return View(windowedModel);
        }

    
        [HttpPost]
        public ActionResult SendWindowedLetter(SendWindowedLetterViewModel model)
        {
            Order order = new Order();

            order.Customer = SessionHelper.Customer;
            AddressInfo addressInfo = new AddressInfo();

            addressInfo.Country = countryService.GetCountryById(int.Parse(model.SelectedCountry));

            LetterDetails letterDetail = new LetterDetails()
            {
                LetterColor = LetterColor.Color,
                LetterPaperWeight = LetterPaperWeight.Eight,
                LetterProcessing = LetterProcessing.Dull,
                LetterSize = LetterSize.A4,
                LetterType = LetterType.Windowed
            };

            Letter letter = new Letter()
            {
                LetterDetails = letterDetail,
                ToAddress = addressInfo,
            };

            logger.DebugFormat("upload file key: {0}", model.UploadFile);
            letter.LetterContent.Path = model.UploadFile;
            
            if (System.IO.File.Exists(PathHelper.GetAbsoluteFile(letter.LetterContent.Path)))
            {
                letter.LetterContent.Content =
                    System.IO.File.ReadAllBytes(PathHelper.GetAbsoluteFile(letter.LetterContent.Path));
            }
            
            var price = priceService.GetPriceBySpecification(new PriceSpecification()
            {
                CountryId = addressInfo.Country.Id,
                LetterColor = LetterColor.Color,
                LetterProcessing = LetterProcessing.Dull,
                LetterSize = LetterSize.A4,
                LetterType = LetterType.Pres,
                LetterPaperWeight = LetterPaperWeight.Eight,
                PageCount = letter.LetterContent.PageCount,
                OfficeId = SessionHelper.Customer.Organisation.RequiredOfficeId.HasValue ? SessionHelper.Customer.Organisation.RequiredOfficeId.Value : 0
            });

            
            price.VatPercentage = SessionHelper.Customer.VatPercentage();
            var officeProductId = price.OfficeProductId;
            letter.OfficeId = officeProductService.GetOfficeProductById(officeProductId).OfficeId;

            order.OrderLines.Add(new OrderLine()
            {
                BaseProduct = letter,
                ProductType = ProductType.Letter,
                Price = new Price()
                {
                    PriceExVat = price.Total
                }
            });
            order.OrderLines.Add(new OrderLine()
            {
                BaseProduct = letter,
                ProductType = ProductType.Payment,
                PaymentMethodId = 2, // Credit
                Price = new Price()
                {
                    PriceExVat = price.Total
                }
            });

            var updated_order = orderService.Create(order);

            string redirectUrl = paymentService.Process(updated_order);

            if (string.IsNullOrEmpty(redirectUrl))
            {
                DashboardViewModel dashboardViewModel = new DashboardViewModel();
                return RedirectToAction("Index", "User", dashboardViewModel);
            }

            return Redirect(redirectUrl);
        }

        #endregion

        #region Send letter pressed (with address)

        [HttpGet]
        public ActionResult SendALetter()
        {
            if (SessionHelper.Customer.CreditsLeft <= 0.0m)
            {
                return View("Credits");
            }

            CreateSingleLetterModel model = new CreateSingleLetterModel();
            model.Email = SessionHelper.Customer.Email;

            if (SessionHelper.Customer.CreditLimit < SessionHelper.Customer.Credit)
            {
                model.HasCredits = true;
            }


            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString(),
                };

                model.Countries.Add(selectedItem);
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
                addressInfo.State = model.DestinationState;
                addressInfo.City = model.DestinationCity;
                addressInfo.Country = countryService.GetCountryById(model.DestinationCountry);
                addressInfo.Zipcode = model.ZipCode;


                LetterDetails letterDetail = new LetterDetails()
                {
                    LetterColor = LetterColor.Color,
                    LetterPaperWeight = LetterPaperWeight.Eight,
                    LetterProcessing = LetterProcessing.Dull,
                    LetterSize = (LetterSize)model.LetterSize,
                    LetterType = LetterType.Pres
                };

                Letter letter = new Letter()
                {
                    LetterDetails = letterDetail,
                    ToAddress = addressInfo
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

                var price = priceService.GetPriceBySpecification(new PriceSpecification()
                {
                    CountryId = addressInfo.Country.Id,
                    LetterColor = LetterColor.Color,
                    LetterProcessing = LetterProcessing.Dull,
                    LetterSize = LetterSize.A4,
                    LetterType = LetterType.Pres,
                    LetterPaperWeight = LetterPaperWeight.Eight,
                    PageCount = letter.LetterContent.PageCount,
                    OfficeId = SessionHelper.Customer.Organisation.RequiredOfficeId.HasValue ? SessionHelper.Customer.Organisation.RequiredOfficeId.Value : 0
                });

                price.VatPercentage = SessionHelper.Customer.VatPercentage();

                var officeProductId = price.OfficeProductId;
                letter.OfficeId = officeProductService.GetOfficeProductById(officeProductId).OfficeId;

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
                        PriceExVat = price.Total
                    }
                });

                var rest = addCouponlines(price, coupon, order);

                if (rest > 0)
                {
                    order.OrderLines.Add(new OrderLine()
                    {
                        ProductType = ProductType.Payment,
                        PaymentMethodId = 2, // Credit
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
                    DashboardViewModel dashboardViewModel = new DashboardViewModel();
                    return RedirectToAction("Index", "User", dashboardViewModel);
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
            var organisationViewModel = new EditOrganisationViewModel();

            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString(),
                };

                if (country.Id == SessionHelper.Customer.Organisation.Address.Country.Id)
                {
                    selectedItem.Selected = true;
                }

                organisationViewModel.Countries.Add(selectedItem);
            }

            var organisationId = SessionHelper.Customer.Organisation.Id;
            var organisation = organisationService.GetOrganisationById(organisationId);

            organisationViewModel.OrganisationId = organisation.Id;
            organisationViewModel.OrganisationName = organisation.Name;
            organisationViewModel.Address1 = organisation.Address.Address1;
            organisationViewModel.Address2 = organisation.Address.Address2;
            organisationViewModel.State = organisation.Address.State;
            organisationViewModel.City = organisation.Address.City;
            organisationViewModel.ZipCode = organisation.Address.Zipcode;
            organisationViewModel.VatNumber = organisation.Address.VatNr;
    
            

            return View(organisationViewModel);
        }

        [HttpPost]
        public ActionResult EditOrganisation(EditOrganisationViewModel editOrganisationView)
        {
            var organisation = organisationService.GetOrganisationById(editOrganisationView.OrganisationId);

            organisation.Name = editOrganisationView.OrganisationName;
            
            organisation.Address.Address1 = editOrganisationView.Address1;
            organisation.Address.Address2 = editOrganisationView.Address2;
            organisation.Address.City = editOrganisationView.City;
            organisation.Address.Zipcode = editOrganisationView.ZipCode;
            organisation.Address.State = editOrganisationView.State;
            organisation.Address.VatNr = editOrganisationView.VatNumber;
            organisation.Address.Country = countryService.GetCountryById(int.Parse(editOrganisationView.SelectedCountry));
            

            organisationService.Update(organisation);

            var customer = customerService.GetCustomerById(SessionHelper.Customer.Id);
            SessionHelper.Customer = customer;
            FormsAuthentication.SetAuthCookie(customer.Id.ToString(), true);

            ViewData.Add("status","Organisation has now been updated");

            return View(editOrganisationView);
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
                    Value = country.Id.ToString(),
                };

                if (country.Id == SessionHelper.Customer.PreferedCountryId)
                {
                    selectedItem.Selected = true;
                }

                orgView.Countries.Add(selectedItem);
            }

            return View(orgView);
        }

        [HttpPost]
        public ActionResult CreateOrganisation(CreateOrganisationViewModel model)
        {
            var country = countryService.GetCountryById(int.Parse(model.SelectedCountry));

            var organisation = new Organisation();
            organisation.Name = model.OrganisationName;
            organisation.Address.Address1 = model.Address1;
            organisation.Address.Address2 = model.Address2;
            organisation.Address.City = model.City;
            organisation.Address.Zipcode = model.ZipCode;
            organisation.Address.State = model.State;
            organisation.IsPrivate = false;
            organisation.Address.Country = country;

            var stored_organisation = organisationService.Create(organisation);


            var addressList = new AddressList()
            {
                AddressInfo = new AddressInfo()
                {
                    Organisation = model.OrganisationName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    City = model.City,
                    Zipcode = model.ZipCode,
                    VatNr = model.VatNumber,
                    State = model.State,
                    Country = country,   
                },
                OrganisationId = stored_organisation.Id
            };

            organisationService.Create(addressList);
            
            var customer = customerService.GetCustomerById(SessionHelper.Customer.Id);
            customer.Organisation = stored_organisation;
            customer.OrganisationRole = OrganisationRole.Administrator;
            
            var updated_customer = customerService.Update(customer);
            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(customer.Id.ToString(), true);

            var profile_model = new DashboardViewModel();
            buildOverviewModel(profile_model);
            return View("Index", profile_model);
        }

        public ActionResult SkipOrganisation()
        {
            var organisation = new Organisation();
            organisation.Name = SessionHelper.Customer.Email;
            organisation.IsPrivate = true;
            organisation.Address.Country = SessionHelper.Customer.CustomerInfo.Country;

            var stored_organisation = organisationService.Create(organisation);

            var customer = customerService.GetCustomerById(SessionHelper.Customer.Id);
            customer.Organisation = stored_organisation;
            customer.OrganisationRole = OrganisationRole.Administrator;
            var updated_customer = customerService.Update(customer);

            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(customer.Id.ToString(), true);

            var profile_model = new DashboardViewModel();
            buildOverviewModel(profile_model);
            return View("Index", profile_model);
        }

        public ActionResult EditOrganisationSettings()
        {
            var customer = customerService.GetCustomerById(SessionHelper.Customer.Id);
            var editViewModel = new EditOrganisationSettingsViewModel();
            editViewModel.OrganisationId = customer.Organisation.Id;
            editViewModel.Password = customer.Password;
            editViewModel.Email = customer.Email;


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

                if (country.Id == SessionHelper.Customer.PreferedCountryId)
                {
                    selectedItem.Selected = true;
                }

                editViewModel.Countries.Add(selectedItem);
            }

            editViewModel.LetterTypes = ControllerHelpers.GetEnumSelectList<LetterType>().ToList();

            return View(editViewModel);
        }

        [HttpPost]
        public ActionResult EditOrganisationSettings(EditOrganisationSettingsViewModel organisationSettings)
        {
            // user settings
            var customer = customerService.GetCustomerById(SessionHelper.Customer.Id);

            if (!string.IsNullOrEmpty(organisationSettings.Password))
            {
                customer.Password = SHA1PasswordEncryptor.Encrypt(organisationSettings.Password);    
            }
            
            // organisation settings
            var organisation = organisationService.GetOrganisationById(organisationSettings.OrganisationId);
            organisation.OrganisationSettings.PreferedCountryId = int.Parse(organisationSettings.PreferedCountry);
            organisation.OrganisationSettings.LetterType = (LetterType)organisationSettings.LetterType;
            organisationService.Update(organisation);

            var updated_customer = customerService.Update(customer);
            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(customer.Id.ToString(), true);


            ViewData.Add("status", "Settings has now been updated");

            return View(organisationSettings);
        }

        public ActionResult EditContacts()
        {
            var editContactsModel = new EditContactsViewModel();
            buildContactsModel(editContactsModel);
            return View(editContactsModel);
        }

        [HttpPost]
        public ActionResult EditContacts(EditContactsViewModel editContacts)
        {
            if (editContacts.NewContact != null)
            {
                var addressList = new AddressList();
                addressList.AddressInfo.Address1 = editContacts.NewContact.Address1;
                addressList.AddressInfo.Address2 = editContacts.NewContact.Address2;
                addressList.AddressInfo.Zipcode = editContacts.NewContact.ZipCode;
                addressList.AddressInfo.City = editContacts.NewContact.City;
                addressList.AddressInfo.Organisation = editContacts.NewContact.OrganisationName;
                addressList.AddressInfo.State = editContacts.NewContact.State;
                addressList.AddressInfo.AttPerson = string.Empty;
                addressList.AddressInfo.VatNr = editContacts.NewContact.VatNumber;
                addressList.AddressInfo.Country = countryService.GetCountryById(int.Parse(editContacts.NewContact.SelectedCountry));
               
                addressList.OrganisationId = editContacts.OrganisationId;
                

                organisationService.Create(addressList);

                ViewData.Add("status", "New address has been added");
            }

            var editContactsModel = new EditContactsViewModel();
            buildContactsModel(editContactsModel);

            var updated_customer = customerService.Update(SessionHelper.Customer);
            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(SessionHelper.Customer.Id.ToString(), true);


            return View(editContactsModel);
        }

        public ActionResult EditSingleContact(int organisationContactId)
        {
            var addressList = organisationService.GetAddressListById(organisationContactId);

            return View(buildContactViewModel(addressList));
        }

        [HttpPost]
        public ActionResult EditSingleContact(ContactViewModel contact)
        {
            var addressList = organisationService.GetAddressListById(contact.AddressListId);
            addressList.AddressInfo.Address1 = contact.Address1;
            addressList.AddressInfo.Address2 = contact.Address2;
            addressList.AddressInfo.Zipcode = contact.ZipCode;
            addressList.AddressInfo.City = contact.City;
            addressList.AddressInfo.Organisation = contact.OrganisationName;
            addressList.AddressInfo.State = contact.State;
            addressList.AddressInfo.AttPerson = string.Empty;
            addressList.AddressInfo.VatNr = contact.VatNumber;

            var updated_addressList = organisationService.Update(addressList);

            ViewData.Add("status", "The address has been updated");

            var model = buildContactViewModel(addressList);

            var updated_customer = customerService.Update(SessionHelper.Customer);
            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(SessionHelper.Customer.Id.ToString(), true);


            return View(model);
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
            
            foreach (var possiblePaymentMethod in possiblePaymentMethods)
            {
                model.PaymentMethods.Add(new SelectListItem()
                {
                    Text = possiblePaymentMethod.Name,
                    Value = possiblePaymentMethod.Id.ToString()
                });
            }


            return View(model);
        }




        [HttpPost]
        public ActionResult Credits(CreditsViewModel model)
        {
            var selectedPaymentMethod = paymentService.GetPaymentMethodById(int.Parse(model.SelectedPaymentMethod));
            var credit = new Credit();

            var creditLine = new OrderLine()
            {
                Quantity = model.PurchaseAmount,
                ProductType = ProductType.Credit,
                BaseProduct = credit,
                Price = new Price()
                {
                    PriceExVat = model.PurchaseAmount,
                    VatPercentage = SessionHelper.Customer.VatPercentage()
                }
            };

            var paymentLine = new OrderLine()
            {
                PaymentMethodId = selectedPaymentMethod.Id,
                ProductType = ProductType.Payment,
                Price = new Price()
                {
                    PriceExVat = model.PurchaseAmount,
                    VatPercentage = SessionHelper.Customer.VatPercentage()
                }
            };

            Order order = new Order
            {
                Price = new Price()
                {
                    PriceExVat = model.PurchaseAmount,
                    VatPercentage = SessionHelper.Customer.VatPercentage()
                },
                Customer = SessionHelper.Customer
            };
            order.OrderLines.Add(creditLine);
            order.OrderLines.Add(paymentLine);

            var placed_order = orderService.Create(order);
            string redirectUrl = paymentService.Process(placed_order);

            var updated_customer = customerService.Update(SessionHelper.Customer);
            SessionHelper.Customer = updated_customer;
            FormsAuthentication.SetAuthCookie(SessionHelper.Customer.Id.ToString(), true);


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
            return Json("OK",JsonRequestBehavior.AllowGet);
        }

        #region "Invoices"

        [HttpGet]
        public ActionResult InvoiceOverview()
        {
            var customer = SessionHelper.Customer;

            InvoiceOverviewViewModel invoiceOverview = new InvoiceOverviewViewModel();
            invoiceOverview.DateFrom = DateTime.Now.AddDays(-180).Date;
            invoiceOverview.DateTo = DateTime.Now.Date;
            invoiceOverview.InvoiceSnippets = getInvoiceSnippets(invoiceOverview.DateFrom,
                invoiceOverview.DateTo,
                customer.Organisation.Id);

            return View(invoiceOverview);
        }

        [HttpGet]
        public ActionResult DeleteInvoice(Guid id)
        {
            var invoice = invoiceService.GetInvoiceById(id);
            invoiceService.Delete(invoice);

            var dashboardModel = new DashboardViewModel();
            buildOverviewModel(dashboardModel);
            return View("Index", dashboardModel);
        }

        #endregion

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
                Id = letter.Id,
                Price = order.Price

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
                OrganisationId = organisationId,
                Take = 9999
            });
            var models = new List<InvoiceSnippetViewModel>();

            foreach (var invoice in invoices)
            {
                models.Add(new InvoiceSnippetViewModel()
                {
                    DateCreated = invoice.DateCreated,
                    OrderNumber = invoice.InvoiceNumber,
                    TotalPrice = invoice.PriceTotal,
                    InvoiceGuid = invoice.Guid,
                    Status = invoice.InvoiceStatus
                });
            }


            return models;
        }

        private void buildOverviewModel(DashboardViewModel model)
        {
            var orders = orderService.GetOrderBySpecification(new OrderSpecification()
            {
                UserId = SessionHelper.Customer.Id
            }).OrderByDescending(c => c.DateCreated);

            model.Orders = getOrderViewModel(orders);
            model.Customer = SessionHelper.Customer;
            model.LetterType = SessionHelper.Customer.DefaultLetterType;

            var unpaidInvoices = invoiceService.GetInvoiceBySpecification(new InvoiceSpecification()
            {
                OrganisationId = SessionHelper.Customer.Organisation.Id,
                InvoiceStatus = InvoiceStatus.Created
            });

            if (unpaidInvoices != null && unpaidInvoices.Any())
            {
                model.UnpaidInvoices = new InvoiceOverviewViewModel();
                foreach (var unpaidInvoice in unpaidInvoices)
                {
                    model.UnpaidInvoices.InvoiceSnippets.Add(new InvoiceSnippetViewModel()
                    {
                        DateCreated = unpaidInvoice.DateCreated,
                        InvoiceGuid = unpaidInvoice.Guid,
                        OrderNumber = unpaidInvoice.InvoiceNumber,
                        Status = unpaidInvoice.InvoiceStatus,
                        TotalPrice = unpaidInvoice.PriceTotal
                    });
                }    
            }
        }

        private void buildContactsModel(EditContactsViewModel model)
        {
            var organisation = SessionHelper.Customer.Organisation;

            foreach (var addressList in organisation.AddressList)
            {
                model.Contacts.Add(getContactViewModel(addressList));
            }
            model.OrganisationId = organisation.Id;

            var countries = countryService.GetCountryBySpecificaiton(new CountrySpecification()
            {
                Take = 999
            });

            foreach (var country in countries)
            {
                var selectedItem = new SelectListItem()
                {
                    Text = country.Name,
                    Value = country.Id.ToString(),
                };

                if (country.Id == SessionHelper.Customer.PreferedCountryId)
                {
                    selectedItem.Selected = true;
                }

                model.NewContact.Countries.Add(selectedItem);
            }
        }

        private ContactViewModel getContactViewModel(AddressList addressList)
        {
            return new ContactViewModel()
            {
                Address1 = addressList.AddressInfo.Address1,
                Address2 = addressList.AddressInfo.Address2,
                City = addressList.AddressInfo.City,
                ZipCode = addressList.AddressInfo.Zipcode,
                State = addressList.AddressInfo.State,
                OrganisationName = addressList.AddressInfo.Organisation,
                AddressListId = addressList.Id,
                
            };
        }


        private ContactViewModel buildContactViewModel(AddressList addressList)
        {
            ContactViewModel model = new ContactViewModel();
            model.Address1 = addressList.AddressInfo.Address1;
            model.Address2 = addressList.AddressInfo.Address2;
            model.State = addressList.AddressInfo.State;
            model.City = addressList.AddressInfo.City;
            model.ZipCode = addressList.AddressInfo.Zipcode;
            model.State = addressList.AddressInfo.State;
            model.OrganisationName = addressList.AddressInfo.Organisation;
            model.AddressListId = addressList.Id;
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

                if (country.Id == addressList.AddressInfo.Country.Id)
                {
                    selectedItem.Selected = true;
                }

                model.Countries.Add(selectedItem);
            }

            return model;
        }


        #endregion
    }
}
