using System;
using Contracts.DAL.Base.Repositories;
using Contracts.Domain.Base;

namespace Contracts.BLL.Base.Services
{
    public interface IBaseEntityService<TBllEntity, TDalEntity>: IBaseEntityService<TBllEntity, TDalEntity, Guid>
            where TBllEntity : class, IDomainEntityId
            where TDalEntity : class, IDomainEntityId
        {
        
        }
    
        public interface IBaseEntityService<TBllEntity, TDalEntity, TKey> : IBaseService, IBaseRepository<TBllEntity, TKey>
            where TBllEntity : class, IDomainEntityId<TKey> 
            where TDalEntity : class, IDomainEntityId<TKey> 
            where TKey : IEquatable<TKey>
        {
        
        }
}