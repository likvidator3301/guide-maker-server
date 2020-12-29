using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    [JsonObject]
    public sealed class GuideDescription
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }
    }
}
