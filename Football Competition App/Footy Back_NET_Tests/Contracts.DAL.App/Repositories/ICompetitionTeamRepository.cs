using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ICompetitionTeamRepository: IBaseRepository<DALAppDTO.CompetitionTeam>, ICompetitionTeamRepositoryCustom<DALAppDTO.CompetitionTeam>
    {
    }

    public interface ICompetitionTeamRepositoryCustom<TEntity>
    {
        
        public Task<bool> BelongsToUserId(Guid userId, Guid id, bool noTracking = true);
        
        public Task<bool> TeamExistsAtCompetition(Guid teamId, Guid competitionId, bool noTracking = true);

        public Task<TEntity?> FirstOrDefaultAsyncWithEntities(Guid id, Guid userId = default, bool noTracking = true);

        public Task<IEnumerable<TEntity>> GetAllOrganiserCompetitions(Guid userId, bool noTracking = true);
        
        public Task<IEnumerable<TEntity>> GetAllByCompetitonId(Guid competitionId, bool noTracking = true);
        
        public Task<IEnumerable<TEntity>> GetAllByTeamId(Guid teamId, bool noTracking = true);
        
        public Task<IEnumerable<Guid>> GetAllTeamIdsByCompetitionId(Guid? competitionId, bool noTracking = true);
    }
}