using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LetterAmazer.Business.Services.Domain.DeliveryJobs;
using LetterAmazer.Business.Services.Domain.FulfillmentPartners;
using LetterAmazer.Business.Services.Domain.Fulfillments;
using LetterAmazer.Business.Services.Domain.Letters;
using LetterAmazer.Business.Services.Domain.OfficeProducts;
using LetterAmazer.Business.Services.Domain.Offices;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Products;
using LetterAmazer.Business.Services.Services.FulfillmentJobs;
using log4net;
using NCrontab;

namespace LetterAmazer.Business.Services.Services
{
    public class DeliveryJobService : IDeliveryJobService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DeliveryJobService));

        private IOrderService orderService;
        private IFulfillmentService fulfillmentService;
        private IOfficeProductService officeProductService;
        private IOfficeService officeService;
        private IFulfillmentPartnerService fulfillmentPartnerService;
        private ILetterService letterService;


        public DeliveryJobService(IOrderService orderService,IFulfillmentService fulfillmentService, IOfficeProductService officeProductService, 
            IOfficeService officeService, IFulfillmentPartnerService fulfillmentPartnerService,ILetterService letterService)
        {
            this.orderService = orderService;
            this.fulfillmentService = fulfillmentService;
            this.officeProductService = officeProductService;
            this.officeService = officeService;
            this.fulfillmentPartnerService = fulfillmentPartnerService;
            this.letterService = letterService;
        }

        public void Execute()
        {
            var relevantOrders = orderService.GetOrderBySpecification(new OrderSpecification()
            {
                OrderStatus = new List<OrderStatus>()
                    {
                        OrderStatus.Paid,
                        OrderStatus.InProgress,
                        OrderStatus.Created
                    }
            });


            var letters = getLettersFromOrders(relevantOrders);

            // no letters to send in this batch
            if (!letters.Any())
            {
                return;
            }

            try
            {
                var lettersByPartnerJob = getLettersByPartnerJob(letters);

                foreach (var entity in lettersByPartnerJob)
                {
                    processDelivery(entity);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void processDelivery(KeyValuePair<int, List<Letter>> entity)
        {
            var fulfillmentPartner = fulfillmentPartnerService.GetFulfillmentPartnerById(entity.Key);
       
            IFulfillmentService fulfillmentService = null;
            if (fulfillmentPartner.PartnerJob == PartnerJob.Jupiter)
            {
                fulfillmentService = new JupiterService(letterService, orderService);
            }
            else if (fulfillmentPartner.PartnerJob == PartnerJob.PostalMethods)
            {
                fulfillmentService = new PostalMethodsService(letterService, orderService);
            }

            if (fulfillmentService != null)
            {
                var schedule = CrontabSchedule.Parse(fulfillmentPartner.CronInterval);
                var exDate = schedule.GetNextOccurrence(DateTime.Now.AddHours(-1));

                if (exDate < DateTime.Now)
                {
                    fulfillmentService.Process(entity.Value);
                }
            }
        }

        private static List<Letter> getLettersFromOrders(List<Order> relevantOrders)
        {
            var letters = new List<Letter>();
            foreach (var relevantOrder in relevantOrders)
            {
                foreach (var letter in relevantOrder.OrderLines)
                {
                    if (letter.ProductType == ProductType.Letter)
                    {
                        var baseProduct = (Letter) letter.BaseProduct;
                        if (baseProduct.LetterStatus == LetterStatus.Created)
                        {
                            letters.Add(baseProduct);
                        }
                    }
                }
            }
            return letters;
        }

        private Dictionary<int, List<Letter>> getLettersByPartnerJob(IEnumerable<Letter> letters)
        {
            var letterByPartnerJob = new Dictionary<int, List<Letter>>();
            foreach (var letter in letters)
            {
                var office = officeService.GetOfficeById(letter.OfficeId);
                var partner = fulfillmentPartnerService.GetFulfillmentPartnerById(office.FulfillmentPartnerId);

                if (!letterByPartnerJob.ContainsKey(partner.Id))
                {
                    letterByPartnerJob.Add(partner.Id, new List<Letter>());
                }
                letterByPartnerJob[partner.Id].Add(letter);
            }
            return letterByPartnerJob;
        }
    }
}
