using System.Threading.Tasks;
using GuideMaker.Repository.Filters;
using GuideMaker.Repository.Models;
using JetBrains.Annotations;

namespace GuideMaker.Repository.Repositories
{
    [PublicAPI]
    public interface IGuideRepository
    {
        Task<bool> ExistsAsync(string id);

        Task<Guide> GetAsync(string id);

        Task<Guide[]> SearchAsync(IFilter filter, int take, int skip);

        Task SaveAsync(Guide guide);
    }
}
