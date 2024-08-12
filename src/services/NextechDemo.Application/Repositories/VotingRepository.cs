using CouncilVoting.Domain.Entities;
using CouncilVoting.Infrastructure.Persistance;
using CouncilVoting.Infrastructure.Respositories;

namespace CouncilVoting.Application.Repositories
{
    public class VotingRepository : BaseRepository<Voting>, IVotingRepository
    {
        public VotingRepository(CouncilVotingContext context) : base(context)
        {
        }
    }
}
