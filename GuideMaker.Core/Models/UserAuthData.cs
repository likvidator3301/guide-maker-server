using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    public sealed class UserAuthData
    {
        [JsonProperty("login", Required = Required.Always)]
        public string Login { get; set; }

        [JsonProperty("password", Required = Required.Always)]
        public string Password { get; set; }
    }
}
