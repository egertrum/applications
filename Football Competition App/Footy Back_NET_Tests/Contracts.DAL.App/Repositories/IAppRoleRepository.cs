using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App.Identity;

namespace Contracts.DAL.App.Repositories
{
    public interface IAppRoleRepository<TEntity>
    {
        public Task<TEntity> FirstOrDefaultAsync(Guid userId = default, bool noTracking = true);
        
        public Task<IEnumerable<TEntity>> GetAllAsync(Guid userId = default, bool noTracking = true);
    }
}