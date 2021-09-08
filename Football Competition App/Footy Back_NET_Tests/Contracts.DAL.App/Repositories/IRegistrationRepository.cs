using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IRegistrationRepository: IBaseRepository<DALAppDTO.Registration>, IRegistrationRepositoryCustom<DALAppDTO.Registration>
    {
    }

    public interface IRegistrationRepositoryCustom<TEntity>
    {
        public Task<bool> CompetitionRegisteredToUser(Guid userId, Guid compId, bool noTracking = true);

        public Task<TEntity> GetByCompetitionId(Guid compId, bool noTracking = true);
    }
}