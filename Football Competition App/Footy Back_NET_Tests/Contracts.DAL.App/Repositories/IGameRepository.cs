using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IGameRepository: IBaseRepository<DALAppDTO.Game>, IGameRepositoryCustom<DALAppDTO.Game>
    {
    }

    public interface IGameRepositoryCustom<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAllUserGames(Guid userId);

        public Task<IEnumerable<TEntity>> GetAllOrganiserGames(Guid userId);
        
        public Task<IEnumerable<TEntity>> GetAllByCompetitionId(Guid competitionId);
    }
}