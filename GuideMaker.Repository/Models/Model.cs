using Newtonsoft.Json;

namespace GuideMaker.Repository.Models
{
    [JsonObject]
    public abstract class Model
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }
    }
}
