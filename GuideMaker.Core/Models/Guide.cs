using Newtonsoft.Json;

namespace GuideMaker.Core.Models
{
    [JsonObject]
    public sealed class Guide
    {
        [JsonProperty("description", Required = Required.Always)]
        public GuideDescription Description { get; set; }

        [JsonProperty("slides", Required = Required.Always)]
        public Slide[] Slides { get; set; }

        [JsonProperty("ownerId", Required = Required.Always)]
        public string OwnerId { get; set; }

        [JsonProperty("likes", Required = Required.Always)]
        public string[] Likes { get; set; }

        [JsonProperty("tags", Required = Required.Always)]
        public string[] Tags { get; set; }
    }
}
