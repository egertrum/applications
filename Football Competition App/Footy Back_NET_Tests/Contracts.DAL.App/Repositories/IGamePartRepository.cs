using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IGamePartRepository: IBaseRepository<DALAppDTO.GamePart>, IGamePartRepositoryCustom<DALAppDTO.GamePart>
    {
    }

    public interface IGamePartRepositoryCustom<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAllByGameId(Guid gameId);

        public Task<int> GetNormalTimeLength(Guid gameId);
        
        public Task<int?> GetExtraTimeLength(Guid gameId);
    }
}