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
    public class EventRepository: BaseRepository<DAL.App.DTO.Event, Domain.App.Event, AppDbContext>, IEventRepository
    {
        public EventRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new EventMapper(mapper))
        {
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.Event>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(e => e.Game).ThenInclude(g => g!.Home)
                .Include(g => g.Game).ThenInclude(g => g!.Away)
                .Include(g => g.GamePart).ThenInclude(g => g!.GamePartType);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Event?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(e => e.Game).ThenInclude(g => g!.Home)
                .Include(g => g.Game).ThenInclude(g => g!.Away)
                .Include(g => g.GamePart).ThenInclude(g => g!.GamePartType);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
    }
}