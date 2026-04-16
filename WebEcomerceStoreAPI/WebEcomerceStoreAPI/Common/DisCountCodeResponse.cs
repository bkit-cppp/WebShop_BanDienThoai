namespace WebEcomerceStoreAPI.Common
{
    public class DisCountCodeResponse
    {
        public Guid DiscountId { get; set; }
        public string Code { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

    }
}
