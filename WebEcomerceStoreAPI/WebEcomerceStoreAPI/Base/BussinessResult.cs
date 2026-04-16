namespace WebEcomerceStoreAPI.Base
{
    public interface IBussinessResult
    {
        int Status { get; set; }
        string ? Message { get; set; }
        object ? Data { get; set; }

    }
    public class BussinessResult: IBussinessResult
    {
        public BussinessResult()
        {
            Status = -1;
            Message = "Action fail";
        }
        public BussinessResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public BussinessResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
        public int Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
