using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Castle.Windsor.Installer;
using LetterAmazer.Business.Services.Domain.Orders;
using LetterAmazer.Business.Services.Domain.Payments;

namespace LetterAmazer.Business.Services.Services.PaymentMethods.Implementations
{
    public class EpayMethod:IPaymentMethod 
    {
        private string payUrl { get; set; }
        private IOrderService orderService { get; set; }

        public EpayMethod(IOrderService orderService)
        {
            this.payUrl = ConfigurationManager.AppSettings.Get("LetterAmazer.Payment.Epay.PayUrl");
            this.orderService = orderService;
        }

        public string Process(Order order)
        {
            string paymentUrl = string.Format(payUrl, order.Id);
            return paymentUrl;
        }

        public void VerifyPayment(Order order)
        {
            throw new NotImplementedException();
        }

        public void CallbackNotification()
        {
            int id = 0;
            int.TryParse(HttpContext.Current.Request.QueryString["orderid"], out id);

            var order = orderService.GetOrderById(id);
            order.OrderStatus = OrderStatus.Paid;
            order.DatePaid = DateTime.Now;
            orderService.ReplenishOrderLines(order);
            orderService.Update(order);

        }

        public void ChargeBacks(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
