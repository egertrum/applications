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
using Country = DAL.App.DTO.Country;

namespace DAL.App.EF.Repositories
{
    public class CountryRepository: BaseRepository<DAL.App.DTO.Country, Domain.App.Country, AppDbContext>, ICountryRepository
    {
        public CountryRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new CountryMapper(mapper))
        {
        }
        
        public override Country Update(DTO.Country country)
        {
            var domainEntity = Mapper.Map(country);

            // load the translations (will loose the dal mapper translations)
            domainEntity!.Name = 
                RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.NameId);
            // set the value from dal entity back to list
            domainEntity!.Name.SetTranslation(country.Name);
            
            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            var dalEntity = Mapper.Map(updatedEntity);
            return dalEntity!;
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.Country>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            var res = await resQuery.ToListAsync();
            
            return res!;
        }

        public override async Task<DAL.App.DTO.Country?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<IEnumerable<DAL.App.DTO.Country>> GetAllWithCountsAsync(bool noTracking = true)
        {
            var query = CreateQuery(default, noTracking);

            var resQuery = query.Select(country => new DAL.App.DTO.Country()
            {
                Id = country.Id,
                PersonsCount = country.Persons!.Count,
                TeamsCount = country.Teams!.Count,
                CompetitionsCount = country.Competitions!.Count
            });

            var res = await resQuery.ToListAsync();
            
            return res;
        }
    }
}