using System.Threading.Tasks;
using GuideMaker.Repository.Filters;
using GuideMaker.Repository.Models;
using JetBrains.Annotations;

namespace GuideMaker.Repository.Repositories
{
    [PublicAPI]
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(string id);

        Task<User> GetAsync(string id);

        Task SaveAsync(User user);

        Task<User[]> SearchAsync(IFilter filter, int take, int skip);
    }
}
