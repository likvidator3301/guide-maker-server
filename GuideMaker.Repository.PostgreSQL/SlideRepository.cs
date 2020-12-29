using GuideMaker.Repository.Models;
using GuideMaker.Repository.Repositories;

namespace GuideMaker.Repository.PostgreSQL
{
    public sealed class SlideRepository : AbstractRepository<Slide>, ISlideRepository
    {
        public SlideRepository(NpgSqlConnectionFactory npgSqlConnectionFactory) : base(npgSqlConnectionFactory, "slides")
        { }
    }
}