namespace WebEcomerceStoreAPI.Common
{
    public class InventoryResponse
    {
        public Guid Id { get; set; }
        
        public int Quantity { get; set; }

        public DateTime LastDated { get; set; }

    }
}
