using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App;
using Microsoft.EntityFrameworkCore;
using CompetitionTeam = DAL.App.DTO.CompetitionTeam;

namespace DAL.App.EF.Repositories
{
    public class CompetitionTeamRepository: BaseRepository<DAL.App.DTO.CompetitionTeam, Domain.App.CompetitionTeam, AppDbContext>, ICompetitionTeamRepository
    {
        public CompetitionTeamRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new CompetitionTeamMapper(mapper))
        {
        }

        
        public override async Task<IEnumerable<DAL.App.DTO.CompetitionTeam>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Team)
                .Include(c => c.Competition);

            if (userId != default)
            {
                query = query.Where(c => c.Team!.AppUserId == userId);
            }

            var resQuery = query
                .Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<bool> TeamExistsAtCompetition(Guid teamId, Guid competitionId, bool noTracking = true)
        {
            var query = CreateQuery();

            var competitionTeam =
                await query.FirstOrDefaultAsync(m => m.TeamId == teamId && m.CompetitionId == competitionId);

            return competitionTeam != null;
        }

        public async Task<DAL.App.DTO.CompetitionTeam?> FirstOrDefaultAsyncWithEntities(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Competition)
                .Include(c => c.Team);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<bool> BelongsToUserId(Guid userId, Guid id, bool noTracking = true)
        {
            var query = CreateQuery();

            var competitionTeam =
                await query.FirstOrDefaultAsync(m => m.Id == id && m.Team!.AppUserId == userId);

            return competitionTeam != null;
        }

        public async Task<IEnumerable<CompetitionTeam>> GetAllOrganiserCompetitions(Guid userId, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Team)
                .Include(c => c.Competition)
                .Where(x => x.Competition!.Registrations!.Any(r => r.AppUserId == userId));

            var resQuery = query
                .Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<IEnumerable<CompetitionTeam>> GetAllByCompetitonId(Guid competitionId, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Team)
                .ThenInclude(t => t!.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(c => c.Competition)
                .Where(x => x.Competition!.Id == competitionId);

            var resQuery = query
                .Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<IEnumerable<CompetitionTeam>> GetAllByTeamId(Guid teamId, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Where(x => x.TeamId == teamId);

            var resQuery = query
                .Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<IEnumerable<Guid>> GetAllTeamIdsByCompetitionId(Guid? competitionId, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query.Where(c => c.CompetitionId == competitionId);
            var res = await query.ToListAsync();
            var teamIds = res.Select(competitionTeam => competitionTeam.TeamId).ToList();

            return teamIds;
        }
    }
}