using Newtonsoft.Json;

namespace GuideMaker.Repository.Models
{
    [JsonObject]
    public sealed class Slide: Model
    {
        [JsonProperty("base64Image", Required = Required.Always)]
        public string Base64Image { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }
    }
}
