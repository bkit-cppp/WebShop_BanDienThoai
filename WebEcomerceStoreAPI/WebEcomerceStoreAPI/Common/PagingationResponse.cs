namespace WebEcomerceStoreAPI.Common
{
    public class PagingationResponse <T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public string? Next { get; set; }
        public bool HasNextPage { get; set; }
    }
}
