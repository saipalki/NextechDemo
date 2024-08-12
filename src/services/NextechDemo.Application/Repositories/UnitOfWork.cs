using CouncilVoting.Infrastructure.Persistance;
using CouncilVoting.Infrastructure.Respositories;

namespace CouncilVoting.Application.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CouncilVotingContext _context;
        public UnitOfWork(CouncilVotingContext context)
        {
            _context = context;
        }
        /// <summary>
        ///     Commit to DB 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Save(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
