using System.Net;

namespace ADSearch.Domain.Items
{
    public class ResultItem
    {
        public bool IsOk { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ResultItem(bool isOk = true, object data = null, string message = "", HttpStatusCode httpStatusCode = HttpStatusCode.OK)
        {
            IsOk = isOk;
            Data = data;
            Message = message;
            StatusCode = httpStatusCode;
        }
    }
}
