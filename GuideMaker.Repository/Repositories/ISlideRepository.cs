using System.Threading.Tasks;
using GuideMaker.Repository.Models;
using JetBrains.Annotations;

namespace GuideMaker.Repository.Repositories
{
    [PublicAPI]
    public interface ISlideRepository
    {
        Task<bool> ExistsAsync(string id);

        Task<Slide> GetAsync(string id);

        Task SaveAsync(Slide slide);
    }
}
