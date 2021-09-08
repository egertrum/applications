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
using Game = DAL.App.DTO.Game;

namespace DAL.App.EF.Repositories
{
    public class GameRepository: BaseRepository<DAL.App.DTO.Game, Domain.App.Game, AppDbContext>, IGameRepository
    {
        public GameRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new GameMapper(mapper))
        {
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.Game>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Away)
                .Include(g => g.Competition)
                .Include(g => g.Home)
                .OrderBy(g => g.Competition!.Name)
                .ThenBy(g => g.KickOffTime);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Game?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Away)
                .Include(g => g.Competition)
                .Include(g => g.Home);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<IEnumerable<Game>> GetAllUserGames(Guid userId)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Away)
                .Include(g => g.Competition)
                .Include(g => g.Home)
                .Where(g => g.Away!.AppUserId == userId || g.Home!.AppUserId == userId)
                .OrderBy(g => g.Competition!.Name)
                .ThenBy(g => g.KickOffTime);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<IEnumerable<Game>> GetAllOrganiserGames(Guid userId)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Away)
                .Include(g => g.Competition)
                .Include(g => g.Home)
                .Where(g => g.Competition!.Registrations!.Any(r => r.AppUserId == userId))
                .OrderBy(g => g.Competition!.Name)
                .ThenBy(g => g.KickOffTime);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<IEnumerable<Game>> GetAllByCompetitionId(Guid competitionId)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Away)
                .Include(g => g.Competition)
                .Include(g => g.Home)
                .Where(g => g.CompetitionId == competitionId)
                .OrderBy(g => g.Competition!.Name)
                .ThenBy(g => g.KickOffTime);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }
    }
}