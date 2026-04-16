namespace WebEcomerceStoreAPI.Entities
{
    public class Product
    {
        public Guid ProductId{ get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int QuantityStock { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<Inventory> Inventories { get; set; }
    }
}
