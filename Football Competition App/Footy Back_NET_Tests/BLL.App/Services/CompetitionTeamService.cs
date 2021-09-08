using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;


namespace BLL.App.Services
{
    public class CompetitionTeamService: BaseEntityService<IAppUnitOfWork, ICompetitionTeamRepository, BLLAppDTO.CompetitionTeam, DALAppDTO.CompetitionTeam>, ICompetitionTeamService
    {
        public CompetitionTeamService(IAppUnitOfWork serviceUow, ICompetitionTeamRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new CompetitionTeamMapper(mapper))
        {
        }

        public async Task<bool> BelongsToUserId(Guid userId, Guid id, bool noTracking = true)
        {
            return await ServiceRepository.BelongsToUserId(userId, id);
        }

        public async Task<bool> TeamExistsAtCompetition(Guid teamId, Guid competitionId, bool noTracking = true)
        {
            return await ServiceRepository.TeamExistsAtCompetition(teamId, competitionId);
        }

        public async Task<BLLAppDTO.CompetitionTeam?> FirstOrDefaultAsyncWithEntities(Guid id, Guid userId = default, bool noTracking = true)
        {
            return Mapper.Map(await ServiceRepository.FirstOrDefaultAsyncWithEntities(id));
        }

        public async Task<IEnumerable<BLLAppDTO.CompetitionTeam>> GetAllOrganiserCompetitions(Guid userId, bool noTracking = true)
        {
            return (await ServiceRepository.GetAllOrganiserCompetitions(userId)).Select(x => Mapper.Map(x))!;
        }

        public async Task<IEnumerable<BLLAppDTO.CompetitionTeam>> GetAllByCompetitonId(Guid competitionId, bool noTracking = true)
        {
            return (await ServiceRepository.GetAllByCompetitonId(competitionId)).Select(x => Mapper.Map(x))!;
        }

        public async Task<IEnumerable<BLLAppDTO.CompetitionTeam>> GetAllByTeamId(Guid teamId, bool noTracking = true)
        {
            return (await ServiceRepository.GetAllByTeamId(teamId)).Select(x => Mapper.Map(x))!;
        }

        public async Task<IEnumerable<Guid>> GetAllTeamIdsByCompetitionId(Guid? competitionId, bool noTracking = true)
        {
            return await ServiceRepository.GetAllTeamIdsByCompetitionId(competitionId);
        }
    }

}