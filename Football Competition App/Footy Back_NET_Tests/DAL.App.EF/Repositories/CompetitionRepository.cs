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
using Competition = DAL.App.DTO.Competition;

namespace DAL.App.EF.Repositories
{
    public class CompetitionRepository: BaseRepository<DAL.App.DTO.Competition, Domain.App.Competition, AppDbContext>, ICompetitionRepository
    {
        public CompetitionRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new CompetitionMapper(mapper))
        {
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.Competition>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(x => x.Registrations)
                .OrderBy(c => c.StartDate);

            if (userId != default)
            {
                query = query.Where(x => x.Registrations!.Any(r => r.AppUserId == userId));
            }
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public override async Task<DAL.App.DTO.Competition?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(p => p.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations);

            var resQuery = query.Select(domainEntity => domainEntity);
            
            var res = Mapper.Map(await resQuery.FirstOrDefaultAsync(m => m!.Id == id));

            return res;
        }

        public async Task<IEnumerable<Competition>> GetAllAsyncWithSearch(Competition? searchCompetition)
        {
            var query = CreateQuery();

            if (searchCompetition != null)
            {
                if (!string.IsNullOrEmpty(searchCompetition.Name))
                {
                    query = query.Where(c => c.Name.Contains(searchCompetition.Name));
                }

                if (searchCompetition.CountryId != Guid.Empty)
                {
                    query = query.Where(c => c.CountryId == searchCompetition.CountryId);
                }

                if (searchCompetition.StartDate != DateTime.MinValue)
                {
                    query = query.Where(c => c.StartDate > searchCompetition.StartDate);
                }   
            }

            query = query
                .Include(c => c.Country)
                .ThenInclude(c => c!.Name)
                .ThenInclude(t => t!.Translations)
                .OrderBy(c => c.StartDate);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }
    }
}