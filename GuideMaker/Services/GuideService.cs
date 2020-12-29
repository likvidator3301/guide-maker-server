using System;
using System.Linq;
using System.Threading.Tasks;
using GuideMaker.Core.Models;
using GuideMaker.Exceptions;
using GuideMaker.Mappers;
using GuideMaker.Repository.Repositories;

namespace GuideMaker.Services
{
    public interface IGuideService
    {
        Task<bool> ExistsAsync(string id);

        Task<Result<Guide>> GetAsync(string id);

        Task<Result<GuideDescription>> SaveAsync(Guide guide);
    }

    internal sealed class GuideService: IGuideService
    {
        private readonly IGuideRepository guideRepository;
        private readonly ISlideRepository slideRepository;

        public GuideService(IGuideRepository guideRepository, ISlideRepository slideRepository)
        {
            this.guideRepository = guideRepository ?? throw new ArgumentNullException(nameof(guideRepository));
            this.slideRepository = slideRepository ?? throw new ArgumentNullException(nameof(slideRepository));
        }

        public Task<bool> ExistsAsync(string id)
        {
            return guideRepository.ExistsAsync(id);
        }

        public async Task<Result<Guide>> GetAsync(string id)
        {
            if (!await guideRepository.ExistsAsync(id))
                throw new NotFoundException($"Guide with id '{id}' not found");

            var dbGuide = await guideRepository.GetAsync(id);
            var slides = dbGuide.Slides.Select(x => slideRepository.GetAsync(x).Result);
            return Result<Guide>.Success(FromDatabaseMapper.MapGuide(dbGuide, slides));
        }

        public async Task<Result<GuideDescription>> SaveAsync(Guide guide)
        {
            var slides = guide.Slides.Select(ToDatabaseMapper.MapSlide).ToArray();
            foreach (var slide in slides) 
                await slideRepository.SaveAsync(slide);

            var dbGuide = ToDatabaseMapper.MapGuide(guide, slides.Select(x => x.Id).ToArray());
            await guideRepository.SaveAsync(dbGuide);

            return Result<GuideDescription>.Success(guide.Description);
        }
    }
}
