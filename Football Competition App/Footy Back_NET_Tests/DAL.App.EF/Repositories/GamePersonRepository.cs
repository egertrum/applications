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

namespace DAL.App.EF.Repositories
{
    public class GamePersonRepository: BaseRepository<DAL.App.DTO.GamePerson, Domain.App.GamePerson, AppDbContext>, IGamePersonRepository
    {
        public GamePersonRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new GamePersonMapper(mapper))
        {
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.GamePerson>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();
            
            query = query
                .Include(g => g.Person)
                .Include(g => g.Role)
                .Include(g => g.Team)
                .Include(g => g.Game).ThenInclude(g => g!.Home)
                .Include(g => g.Game).ThenInclude(g => g!.Away);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.GamePerson?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(g => g.Person)
                .Include(g => g.Role)
                .Include(g => g.Team)
                .Include(g => g.Game).ThenInclude(g => g!.Home)
                .Include(g => g.Game).ThenInclude(g => g!.Away);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public int GetGamesCountForPlayer(Guid personId)
        {
            var query = CreateQuery();

            query = query.Where(g => g.PersonId == personId);

            var count = query.Count();

            return count;
        }
    }
}