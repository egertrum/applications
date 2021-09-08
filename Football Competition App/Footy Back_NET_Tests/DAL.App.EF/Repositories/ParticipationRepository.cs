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
    public class ParticipationRepository: BaseRepository<DAL.App.DTO.Participation, Domain.App.Participation, AppDbContext>, IParticipationRepository
    {
        public ParticipationRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new ParticipationMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DAL.App.DTO.Participation>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(p => p.Event)
                .Include(p => p.Person)
                .Include(p => p.Role)
                .Include(p => p.Team);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Participation?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(p => p.Event)
                .Include(p => p.Person)
                .Include(p => p.Role)
                .Include(p => p.Team);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
    }
}