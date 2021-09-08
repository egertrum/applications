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
using Team = DAL.App.DTO.Team;

namespace DAL.App.EF.Repositories
{
    public class TeamRepository: BaseRepository<DAL.App.DTO.Team, Domain.App.Team, AppDbContext>, ITeamRepository
    {
        public TeamRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TeamMapper(mapper))
        {
        }

        
        public override async Task<IEnumerable<DAL.App.DTO.Team>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId);

            query = query
                .Include(t => t.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Team?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId);

            query = query
                .Include(t => t.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(t => t.AppUser);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        /*
        public async Task<IEnumerable<Team>> GetAllWithUserId(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();
            
            //.Include(c => c.Registrations!.Where(x => x.AppUserId == userId))

            query = query
                .Include(t => t.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(t => t.AppUser)
                .Where(x => x.AppUserId == userId);


            var resQuery = query
                .Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        */

        public async Task<bool> BelongsToUserId(Guid userId, Guid teamId, bool noTracking = true)
        {
            var query = CreateQuery(userId);

            query = query
                .Where(r => r.Id == teamId);

            var res = await query.ToListAsync();

            return res.Count > 0;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsByIds(IEnumerable<Guid> ids, bool noTracking = true)
        {
            var query = CreateQuery();
            
            query = query
                .Where(t => ids.Contains(t.Id));
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity)!);
            
            var res = await resQuery.ToListAsync();

            return res;
        }
    }
}