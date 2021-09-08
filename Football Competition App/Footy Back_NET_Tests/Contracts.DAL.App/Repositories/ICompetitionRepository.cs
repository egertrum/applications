using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ICompetitionRepository: IBaseRepository<DALAppDTO.Competition>, ICompetitionRepositoryCustom<DALAppDTO.Competition>
    {
    }

    public interface ICompetitionRepositoryCustom<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAllAsyncWithSearch(TEntity searchCompetition);
    }
}