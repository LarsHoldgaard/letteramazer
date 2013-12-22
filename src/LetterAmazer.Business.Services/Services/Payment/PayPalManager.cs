

namespace LetterAmazer.Business.Services.Services
{
    public class PayPalManager
    {
        //public string GetPaymentUrl(Order order)
        //{
        //    if (order == null)
        //    {
        //        return string.Empty;
        //    }

        //    decimal volume = order.Price;
        //    string firstName = order.ShippingInfo.PaymentInfo.FirstName;
        //    string lastName = order.ShippingInfo.PaymentInfo.LastName;
        //    string country = order.ShippingInfo.PaymentInfo.Country;
        //    string postal = order.ShippingInfo.PaymentInfo.Postal;
        //    string city = order.ShippingInfo.PaymentInfo.City;
        //    string address = order.ShippingInfo.PaymentInfo.Address;
        //    var id = order.Id;

        //    var volumeForUsd = Math.Round(volume, 2).ToString().Replace(",",".");
        //    var url = string.Format("{0}first_name={1}&last_name={2}&item_name={3}&currency_code=USD&amount={4}&notify_url={5}&cmd=_xclick&country={6}&zip={7}&address1={8}&business={9}&city={11}&custom={10}",
        //            ConfigurationManager.AppSettings.Get("PayPal.Url"), firstName, lastName, "Send a single letter", volumeForUsd, ConfigurationManager.AppSettings.Get("PayPal.Ipn"), country, postal, address, "mcoroklo@gmail.com", id, city);

        //    return url;
        //}
    }
}
