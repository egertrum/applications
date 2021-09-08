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
    public class RoleRepository: BaseRepository<DAL.App.DTO.Role, Domain.App.Role, AppDbContext>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new RoleMapper(mapper))
        {
        }

        public override DAL.App.DTO.Role Update(DTO.Role country)
        {
            var domainEntity = Mapper.Map(country);

            // load the translations (will loose the dal mapper translations)
            domainEntity!.Name = 
                RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.NameId);
            // set the value from dal entity back to list
            domainEntity!.Name.SetTranslation(country.Name);
            
            if (country.Comment != null)
            {
                domainEntity!.Comment = 
                    RepoDbContext.LangStrings
                        .Include(t => t.Translations)
                        .First(x => x.Id == domainEntity.CommentId);
                domainEntity!.Name.SetTranslation(country.Comment);   
            }

            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            var dalEntity = Mapper.Map(updatedEntity);
            return dalEntity!;
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.Role>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations)
                .Include(c => c.Comment)
                .ThenInclude(t => t!.Translations);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Role?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations)
                .Include(c => c.Comment)
                .ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
    }
}