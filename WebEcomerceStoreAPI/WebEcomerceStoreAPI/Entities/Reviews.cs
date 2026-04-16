namespace WebEcomerceStoreAPI.Entities
{
    public class Reviews
    {
        public Guid ReviewId { get; set; }
        public Guid ? UserId { get; set; } 
        public Guid ? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual User Users { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
