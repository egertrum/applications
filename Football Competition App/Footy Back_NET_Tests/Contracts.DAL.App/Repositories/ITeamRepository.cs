using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITeamRepository: IBaseRepository<DALAppDTO.Team>, ITeamRepositoryCustom<DALAppDTO.Team>
    {
    }

    public interface ITeamRepositoryCustom<TEntity>
    {
        public Task<bool> BelongsToUserId(Guid userId, Guid teamId, bool noTracking = true);

        public Task<IEnumerable<TEntity>> GetAllTeamsByIds(IEnumerable<Guid> ids, bool noTracking = true);

    }
}