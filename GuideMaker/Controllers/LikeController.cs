using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Exceptions;
using GuideMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuideMaker.Controllers
{
    [ApiController]
    public class LikeController: BaseController
    {
        private IGuideService guideService;
        public LikeController(IGuideService guideService, IUserService userService) : base(userService)
        {
            this.guideService = guideService ?? throw new ArgumentNullException(nameof(guideService));
        }

        [HttpPost("like/{id}")]
        public async Task<Result<GuideDescription>> LikeAsync([FromRoute] string id)
        {
            if (!await guideService.ExistsAsync(id))
                throw new NotFoundException($"Guide with id '{id}' not found");

            var guide = (await guideService.GetAsync(id)).GetValue();

            var user = await GetUserAsync();
            if (guide.Likes.Contains(user.Id))
                throw new ConflictException($"Guide with id '{id}' already liked");

            var newLikes = guide.Likes.ToList();
            newLikes.Add(user.Id);
            guide.Likes = newLikes.ToArray();

            var guideDescription = await guideService.SaveAsync(guide);

            return Result<GuideDescription>.Success(guideDescription.GetValue());
        }

        [HttpPost("dislike/{id}")]
        public async Task<Result<GuideDescription>> DislikeAsync([FromRoute] string id)
        {
            if (!await guideService.ExistsAsync(id))
                throw new NotFoundException($"Guide with id '{id}' not found");

            var guide = (await guideService.GetAsync(id)).GetValue();

            var user = await GetUserAsync();
            if (!guide.Likes.Contains(user.Id))
                throw new ConflictException($"Guide with id '{id}' was not liked");

            var newLikes = guide.Likes.Where(x => x != user.Id).ToList();
            guide.Likes = newLikes.ToArray();

            var guideDescription = await guideService.SaveAsync(guide);

            return Result<GuideDescription>.Success(guideDescription.GetValue());
        }
    }
}
