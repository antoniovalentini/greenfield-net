namespace Avalentini.Shakesmon.Core.Common
{
    public class BaseResponse
    {
        public bool IsSuccess => string.IsNullOrEmpty(Error);
        public string Error { get; set; }
    }
}
