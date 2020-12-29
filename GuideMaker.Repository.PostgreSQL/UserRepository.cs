using GuideMaker.Repository.Models;
using GuideMaker.Repository.Repositories;

namespace GuideMaker.Repository.PostgreSQL
{
    public sealed class UserRepository : AbstractRepository<User>, IUserRepository
    {
        public UserRepository(NpgSqlConnectionFactory npgSqlConnectionFactory) : base(npgSqlConnectionFactory, "users")
        { }
    }
}