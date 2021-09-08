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
using TeamPerson = DAL.App.DTO.TeamPerson;

namespace DAL.App.EF.Repositories
{
    public class TeamPersonRepository: BaseRepository<DAL.App.DTO.TeamPerson, Domain.App.TeamPerson, AppDbContext>, ITeamPersonRepository
    {
        public TeamPersonRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new TeamPersonMapper(mapper))
        {
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.TeamPerson>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(t => t.Person)
                .Include(t => t.Role)
                .ThenInclude(r => r!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(t => t.Team);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public override async Task<DAL.App.DTO.TeamPerson?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(t => t.Person)
                .Include(t => t.Role)
                .ThenInclude(r => r!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(t => t.Team);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<IEnumerable<TeamPerson>> GetAllWithTeamId(Guid teamId)
        {
            var query = CreateQuery();

            query = query
                .Include(t => t.Person)
                .Include(t => t.Role)
                .ThenInclude(r => r!.Name)
                .ThenInclude(t => t!.Translations)
                .Include(t => t.Team)
                .Where(t => t.TeamId == teamId);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();

            return res!;
        }

        public async Task<bool> IsPossibleToAdd(TeamPerson teamPerson)
        {
            var query = CreateQuery();
            
            var teamPersonFromDb =
                await query.FirstOrDefaultAsync(t => 
                    t.TeamId == teamPerson.TeamId 
                    && t.PersonId == teamPerson.PersonId
                    && t.RoleId == teamPerson.RoleId);

            return teamPersonFromDb == null;
        }
    }
}