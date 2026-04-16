namespace WebEcomerceStoreAPI.Common
{
    public class AddOrUpdateInventoryRequest
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public DateTime LastDated { get; set; }
    }
}
