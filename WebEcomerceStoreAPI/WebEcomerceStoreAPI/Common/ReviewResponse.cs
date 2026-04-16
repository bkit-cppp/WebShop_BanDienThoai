namespace WebEcomerceStoreAPI.Common
{
    public class ReviewResponse
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
