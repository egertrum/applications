using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using GamePart = DAL.App.DTO.GamePart;

namespace DAL.App.EF.Repositories
{
    public class GamePartRepository: BaseRepository<DAL.App.DTO.GamePart, Domain.App.GamePart, AppDbContext>, IGamePartRepository
    {
        public GamePartRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new GamePartMapper(mapper))
        {
        }
        
        public async Task<IEnumerable<GamePart>> GetAllByGameId(Guid gameId)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.GamePartType)
                .ThenInclude(g => g!.Name)
                .ThenInclude(t => t!.Translations)
                .Where(g => g.GameId == gameId);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity)!);

            var res = await resQuery.ToListAsync();
            
            return res!;
        }

        public async Task<int> GetNormalTimeLength(Guid gameId)
        {
            var query = CreateQuery();

            query = query.Where(g => g.GameId == gameId && g.GamePartType!.Short == EGamePartType.FirstHalf);
            
            var res = Mapper.Map(await query.FirstOrDefaultAsync());
            
            return res!.Length;
        }

        public async Task<int?> GetExtraTimeLength(Guid gameId)
        {
            var query = CreateQuery();

            query = query.Where(g => g.GameId == gameId && g.GamePartType!.Short == EGamePartType.ExtraTimeFirstHalf);
            
            var res = Mapper.Map(await query.FirstOrDefaultAsync());
            
            return res?.Length;
        }


        public override async Task<IEnumerable<DAL.App.DTO.GamePart>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Game).ThenInclude(g => g!.Home)
                .Include(e => e.Game).ThenInclude(g => g!.Away)
                .Include(g => g.GamePartType)
                .ThenInclude(g => g!.Name).ThenInclude(t => t!.Translations);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();
            
            return res!;
        }
        
        public override async Task<DAL.App.DTO.GamePart?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();
            
            query = query
                .Include(g => g.Game).ThenInclude(g => g!.Home)
                .Include(e => e.Game).ThenInclude(g => g!.Away)
                .Include(g => g.GamePartType)
                .ThenInclude(g => g!.Name).ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
    }
}