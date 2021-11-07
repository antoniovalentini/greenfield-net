using System.Text.Json;

namespace Royale.Sdk
{
    public static class SdkSerializerOptions
    {
        public static readonly JsonSerializerOptions JsonOptions = new() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
    }
}
