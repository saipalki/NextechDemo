using CouncilVoting.Domain.Entities;
using CouncilVoting.Infrastructure.Persistance;
using CouncilVoting.Infrastructure.Respositories;
using Microsoft.EntityFrameworkCore;

namespace CouncilVoting.Application.Repositories
{
    /// <summary>
    ///     Base repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     DB context
        /// </summary>
        protected readonly CouncilVotingContext _context;
        public BaseRepository(CouncilVotingContext context)
        {
            _context = context;
        }
        /// <summary>
        ///     Create an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        /// <summary>
        ///     Delete an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        /// <summary>
        ///     Get an entity based on id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<T> Get(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        /// <summary>
        ///     Get all in a list
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }
        /// <summary>
        ///     Update an entity
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
