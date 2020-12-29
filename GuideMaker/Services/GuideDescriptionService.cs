using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Repository.Filters;
using GuideMaker.Repository.Repositories;

namespace GuideMaker.Services
{
    public interface IGuideDescriptionService
    {
        Task<Result<GuideDescription[]>> SearchByTagsAsync(string[] tags, int take = 50, int skip = 0);

        Task<Result<GuideDescription[]>> SearchByNameAsync(string name, int take = 50, int skip = 0);

        Task<Result<GuideDescription[]>> SearchGuidesByOwnerId(string ownerId, int take = 50, int skip = 0);

        Task<Result<GuideDescription[]>> SearchLikedAsync(string likeOwnerId, int take = 50, int skip = 0);
    }

    internal sealed class GuideDescriptionService: IGuideDescriptionService
    {
        private readonly IGuideRepository guideRepository;

        public GuideDescriptionService(IGuideRepository guideRepository)
        {
            this.guideRepository = guideRepository ?? throw new ArgumentNullException(nameof(guideRepository));
        }

        public async Task<Result<GuideDescription[]>> SearchByTagsAsync(string[] tags, int take = 50, int skip = 0)
        {
            IFilter filter = null;
            if (tags != null)
            {
                var filters = tags.Select(x => (IFilter) new ComparisonFilter("tags", x, ComparisonOperator.Contains))
                    .ToArray();
                filter = new LogicalFilter(LogicalOperator.And, filters);
            }

            var guides = await guideRepository.SearchAsync(filter, take, skip);
            return Result<GuideDescription[]>.Success(MapGuideDescriptions(guides));
        }

        public async Task<Result<GuideDescription[]>> SearchLikedAsync(string likeOwnerId, int take = 50, int skip = 0)
        {
            var filter = new ComparisonFilter("likes", likeOwnerId, ComparisonOperator.Contains);
            var guides = await guideRepository.SearchAsync(filter, take, skip);
            return Result<GuideDescription[]>.Success(MapGuideDescriptions(guides));
        }

        public async Task<Result<GuideDescription[]>> SearchByNameAsync(string name, int take = 50, int skip = 0)
        {
            var filter = new ComparisonFilter("name", name, ComparisonOperator.Contains);
            var guides = await guideRepository.SearchAsync(filter, take, skip);
            return Result<GuideDescription[]>.Success(MapGuideDescriptions(guides));
        }

        public async Task<Result<GuideDescription[]>> SearchGuidesByOwnerId(string ownerId, int take = 50, int skip = 0)
        {
            var filter = new ComparisonFilter("ownerId", ownerId, ComparisonOperator.Equal);
            var guides = await guideRepository.SearchAsync(filter, take, skip);
            return Result<GuideDescription[]>.Success(MapGuideDescriptions(guides));
        }

        private GuideDescription[] MapGuideDescriptions(IEnumerable<Repository.Models.Guide> guides)
        {
            var descriptions = guides.Select(g => new GuideDescription
            {
                Id = g.Id,
                Description = g.Description,
                Name = g.Name
            }).ToArray();
            return descriptions;
        }
    }
}
