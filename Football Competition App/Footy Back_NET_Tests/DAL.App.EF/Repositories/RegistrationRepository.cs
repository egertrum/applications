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
using Registration = DAL.App.DTO.Registration;

namespace DAL.App.EF.Repositories
{
    public class RegistrationRepository: BaseRepository<DAL.App.DTO.Registration, Domain.App.Registration, AppDbContext>, IRegistrationRepository
    {
        public RegistrationRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new RegistrationMapper(mapper))
        {
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.Registration>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(r => r.Competition)
                .Include(r => r.Team);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Registration?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = RepoDbSet.AsQueryable();

            query = query
                .Include(r => r.Competition)
                .Include(r => r.Team);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<bool> CompetitionRegisteredToUser(Guid userId, Guid compId, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Where(r => r.CompetitionId == compId && r.AppUserId == userId);

            var res = await query.ToListAsync();

            return res.Count > 0;
        }

        public async Task<DAL.App.DTO.Registration> GetByCompetitionId(Guid compId, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Where(r => r.CompetitionId == compId);

            var res = Mapper.Map(await query.FirstOrDefaultAsync());

            return res!;
        }
    }
}