using System.Security.Policy;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Exceptions;
using GuideMaker.Services;
using Microsoft.AspNetCore.Mvc;

namespace GuideMaker.Controllers
{
    [ApiController]
    [Route("guides")]
    public class GuidesController: BaseController
    {
        private IGuideService guideService;

        public GuidesController(IGuideService guideService, IUserService userService) : base(userService)
        {
            this.guideService = guideService;
        }

        [HttpGet("{id}")]
        public async Task<Result<Guide>> GetAsync([FromRoute] string id)
        {
            return await guideService.GetAsync(id);
        }

        [HttpPost]
        public async Task<Result<GuideDescription>> SaveAsync([FromBody] Guide guide)
        {
            var user = await GetUserAsync();
            if (guide.OwnerId != user.Id)
                throw new ForbiddenException("Owner id from guide not equal to authenticated user's id");

            var description = await guideService.SaveAsync(guide);
            return Result<GuideDescription>.Success(description.GetValue());
        }

        [HttpPut("{id}")]
        public async Task<Result<GuideDescription>> UpdateAsync([FromRoute] string id, [FromBody] Guide guide)
        {
            if (id != guide.Description.Id)
                throw new ConflictException("Id from route not equal to id from guide description");

            var user = await GetUserAsync();
            if (guide.OwnerId != user.Id)
                throw new ForbiddenException("Owner id from guide not equal to authenticated user's id");

            if (await guideService.ExistsAsync(id))
                throw new NotFoundException($"Guide with id '{id}' not found");

            await guideService.SaveAsync(guide);
            return Result<GuideDescription>.Success(guide.Description);
        }
    }
}
