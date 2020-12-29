using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    public sealed class TokenInformation
    {
        [JsonProperty("token", Required = Required.Always)]
        public string Token { get; set; }
    }
}
