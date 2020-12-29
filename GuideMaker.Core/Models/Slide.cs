using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    [JsonObject]
    public sealed class Slide
    {
        [JsonProperty("base64Image", Required = Required.Always)]
        public string Base64Image { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }
    }
}
