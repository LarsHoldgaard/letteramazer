namespace LetterAmazer.Business.Services.Domain.FulfillmentPartners
{
    public class FulfilmentPartner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ShopId { get; set; }
        public PartnerJob PartnerJob { get; set; }

        public string CronInterval { get; set; }
    }
}
