using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.DAL.App.Repositories
{
    public interface IAppUserRepository<TEntity>
    {
        public Task<TEntity> FirstOrDefaultAsync(Guid id, bool noTracking = true);
        
        public Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true);
    }
}
