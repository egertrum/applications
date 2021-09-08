using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO.Identity;
using DAL.App.EF.Mappers;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class AppUserRepository : IAppUserRepository<DAL.App.DTO.Identity.AppUser>
    {
        private IMapper _mapper;
        private AppDbContext _ctx;

        public AppUserRepository(AppDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _ctx = dbContext;
        }

        public async Task<AppUser> FirstOrDefaultAsync(Guid id, bool noTracking = true)
        {
            var mapper = new AppUserMapper(_mapper);

            var query = _ctx.Users.AsQueryable();
            
            if (noTracking)
            {
                query = query.AsNoTracking();
            }
            
            var res = mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
            
            return res!;
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync(bool noTracking = true)
        {
            var mapper = new AppUserMapper(_mapper);

            var query = _ctx.Users.AsQueryable();
            
            if (noTracking)
            {
                query = query.AsNoTracking();
            }
            
            var resQuery = query.Select(domainEntity => mapper.Map(domainEntity));
            
            var res = await resQuery.ToListAsync();

            return res!;
        }
    }
}