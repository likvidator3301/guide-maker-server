using System;
using System.Security.Cryptography;
using System.Text;
using GuideMaker.Repository.Models;
using Guide = GuideMaker.Repository.Models.Guide;
using Slide = GuideMaker.Repository.Models.Slide;

namespace GuideMaker.Mappers
{
    internal static class ToDatabaseMapper
    {
        public static Guide MapGuide(Core.Models.Guide serverGuide, string[] slides)
        {
            return new Guide
            {
                Id = serverGuide.Description.Id,
                Description = serverGuide.Description.Description,
                Name = serverGuide.Description.Name,
                Likes = string.Join(';', serverGuide.Likes),
                OwnerId = serverGuide.OwnerId,
                Slides = slides,
                Tags = string.Join(';', serverGuide.Tags)
            };
        }

        public static Slide MapSlide(Core.Models.Slide serverSlide)
        {
            return new Slide
            {
                Id = Guid.NewGuid().ToString(),
                Base64Image = serverSlide.Base64Image,
                Text = serverSlide.Text
            };
        }
    }
}
