using CouncilVoting.Domain.Entities;
using CouncilVoting.Infrastructure.Persistance;
using CouncilVoting.Infrastructure.Respositories;

namespace CouncilVoting.Application.Repositories
{
    public class MeasureRepository : BaseRepository<Measure>, IMeasureRepository
    {
        public MeasureRepository(CouncilVotingContext context) : base(context)
        {
        }
    }
}
