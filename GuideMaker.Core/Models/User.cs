using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    [JsonObject]
    public sealed class User
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
    }
}
