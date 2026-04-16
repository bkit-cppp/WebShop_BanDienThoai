namespace WebEcomerceStoreAPI.Entities
{
    public class Inventory
    {
        public Guid InventoryId { get; set; }
        public Guid ? ProductId { get; set; }
        public virtual Product Products { get; set; }
        public int Quantity { get; set; }
        public DateTime LastDated { get; set; }
    }
}
