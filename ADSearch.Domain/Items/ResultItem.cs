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
    }
}
