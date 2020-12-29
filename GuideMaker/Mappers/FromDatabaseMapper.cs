using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using JetBrains.Annotations;

namespace GuideMaker.Mappers
{
    internal static class FromDatabaseMapper
    {
        public static Guide MapGuide([NotNull] Repository.Models.Guide dbGuide, [ItemNotNull] IEnumerable<Repository.Models.Slide> slides)
        {
            return new Guide
            {
                Description = new GuideDescription
                {
                    Id = dbGuide.Id,
                    Description = dbGuide.Description,
                    Name = dbGuide.Name
                },
                Likes = dbGuide.Likes.Split(";"),
                OwnerId = dbGuide.OwnerId,
                Slides = slides.Select(MapSlide).ToArray(),
                Tags = dbGuide.Tags.Split(";")
            };
        }

        private static Slide MapSlide(Repository.Models.Slide slide)
        {
            return new Slide
            {
                Base64Image = slide.Base64Image,
                Text = slide.Text
            };
        }
    }
}
