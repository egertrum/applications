using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Person = DAL.App.DTO.Person;

namespace DAL.App.EF.Repositories
{
    public class PersonRepository: BaseRepository<DAL.App.DTO.Person, Domain.App.Person, AppDbContext>, IPersonRepository
    {
        public PersonRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new PersonMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DAL.App.DTO.Person>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query
                .Include(p => p.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
        
        public override async Task<DAL.App.DTO.Person?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery(userId, noTracking);

            query = query
                .Include(p => p.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<Person?> FindByIdentificationCode(string idCode, Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId);

            query = query
                .Include(p => p.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.IdentificationCode == idCode));

            return res;
        }

        public async Task<bool> ExistsByIdentificationCode(string idCode, Guid userId, bool noTracking = true)
        {
            var query = CreateQuery(userId);

            var res = await query.FirstOrDefaultAsync(m => m.IdentificationCode == idCode);

            return res != null;
        }

        public async Task<Person?> FirstOrDefaultAsyncWithTeams(Guid id, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(p => p.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(p => p.TeamPersons)
                .ThenInclude(t => t.Team);
            
            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            res!.PersonTeams = new Dictionary<string, int>();

            return res;
        }
    }
}