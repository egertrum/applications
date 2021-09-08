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
    public class TeamService: BaseEntityService<IAppUnitOfWork, ITeamRepository, BLLAppDTO.Team, DALAppDTO.Team>, ITeamService
    {
        public TeamService(IAppUnitOfWork serviceUow, ITeamRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new TeamMapper(mapper))
        {
        }

        public async Task<bool> BelongsToUserId(Guid userId, Guid teamId, bool noTracking = true)
        {
            return await ServiceRepository.BelongsToUserId(userId, teamId);
        }

        public async Task<IEnumerable<BLLAppDTO.Team>> GetAllTeamsByIds(IEnumerable<Guid> ids, bool noTracking = true)
        {
            return (await ServiceRepository.GetAllTeamsByIds(ids)).Select(x => Mapper.Map(x)!);
        }

        public async Task RemoveTeam(Guid teamId, bool noTracking = true)
        {
            var teamMembers = await ServiceUow.TeamPersons.GetAllWithTeamId(teamId);
            var teamCompetitions = await ServiceUow.CompetitionTeams.GetAllByTeamId(teamId);
            
            foreach (var member in teamMembers)
            {
                ServiceUow.TeamPersons.Remove(member);
            }
            foreach (var competitionTeam in teamCompetitions)
            {
                ServiceUow.CompetitionTeams.Remove(competitionTeam);
            }

            await ServiceUow.SaveChangesAsync();
            await ServiceRepository.RemoveAsync(teamId);
            await ServiceUow.SaveChangesAsync();
        }
    }

}