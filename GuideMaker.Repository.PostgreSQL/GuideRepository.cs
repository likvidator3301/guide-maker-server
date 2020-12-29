using GuideMaker.Repository.Models;
using GuideMaker.Repository.Repositories;

namespace GuideMaker.Repository.PostgreSQL
{
    public sealed class GuideRepository: AbstractRepository<Guide>, IGuideRepository
    {
        public GuideRepository(NpgSqlConnectionFactory npgSqlConnectionFactory) : base(npgSqlConnectionFactory, "guides")
        { }
    }
}
