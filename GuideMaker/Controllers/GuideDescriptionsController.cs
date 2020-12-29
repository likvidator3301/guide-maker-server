using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GuideMaker.Controllers
{
    [ApiController]
    [Route("/guides/description")]
    public class GuideDescriptionsController: BaseController
    {
        private readonly IGuideDescriptionService guideDescriptionService;
        private readonly ILogger<GuideDescriptionsController> logger;

        public GuideDescriptionsController(IGuideDescriptionService guideDescriptionService, IUserService userService, ILogger<GuideDescriptionsController> logger) : base(userService)
        {
            this.guideDescriptionService = guideDescriptionService ??
                                           throw new ArgumentNullException(nameof(guideDescriptionService));
            this.logger = logger;
        }

        [HttpGet("all")]
        public async Task<Result<GuideDescription[]>> SearchByTagsAsync([FromQuery] string[] tags = null,
            [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            logger.LogInformation("Searching all guides");
            return await guideDescriptionService.SearchByTagsAsync(tags.Length == 0 ? null : tags, take, skip);
        }

        [HttpGet("my")]
        public async Task<Result<GuideDescription[]>> SearchByOwnerId([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var user = await GetUserAsync();
            return await guideDescriptionService.SearchGuidesByOwnerId(user.Id, take, skip);
        }

        [HttpGet]
        public async Task<Result<GuideDescription[]>> SearchByNameAsync([FromQuery] string name, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            logger.LogInformation("Searching all guides");
            return await guideDescriptionService.SearchByNameAsync(name, take, skip);
        }

        [HttpGet("liked")]
        public async Task<Result<GuideDescription[]>> SearchLikedAsync([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var user = await GetUserAsync();
            return await guideDescriptionService.SearchLikedAsync(user.Id, take, skip);
        }
    }
}
