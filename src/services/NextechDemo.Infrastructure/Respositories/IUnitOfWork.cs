using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CouncilVoting.Infrastructure.Respositories
{
    public interface IUnitOfWork
    {
        Task<bool> Save(CancellationToken cancellationToken = default);
    }
}
