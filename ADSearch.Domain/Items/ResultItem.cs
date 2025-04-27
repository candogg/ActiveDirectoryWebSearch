namespace ADSearch.Domain.Items
{
    /// <summary>
    /// Author: Can DOĞU
    /// </summary>
    public class ResultItem
    {
        public bool IsOk { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }

        public ResultItem(bool isOk = true, object data = null, string message = "")
        {
            IsOk = isOk;
            Data = data;
            Message = message;
        }

        public static ResultItem GenerateError(string message = null)
        {
            return new ResultItem(false, message: message);
        }

        public static ResultItem GenerateSuccess(object data = null, string message = null)
        {
            return new ResultItem(data: data, message: message);
        }
    }
}
