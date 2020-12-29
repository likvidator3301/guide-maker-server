using Newtonsoft.Json;

namespace GuideMaker.Repository.Models
{
    [JsonObject]
    public sealed class User: Model
    {
        [JsonProperty("passwordHash", Required = Required.Always)]
        public string PasswordHash { get; set; }

        [JsonProperty("token", Required = Required.Always)]
        public string Token { get; set; }
    }
}
