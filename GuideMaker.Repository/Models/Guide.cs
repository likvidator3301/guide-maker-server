using Newtonsoft.Json;

namespace GuideMaker.Repository.Models
{
    [JsonObject]
    public class Guide: Model
    {
        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        [JsonProperty("slides", Required = Required.Always)]
        public string[] Slides { get; set; }

        [JsonProperty("ownerId", Required = Required.Always)]
        public string OwnerId { get; set; }

        [JsonProperty("likes", Required = Required.Always)]
        public string Likes { get; set; }

        [JsonProperty("tags", Required = Required.Always)]
        public string Tags { get; set; }
    }
}
