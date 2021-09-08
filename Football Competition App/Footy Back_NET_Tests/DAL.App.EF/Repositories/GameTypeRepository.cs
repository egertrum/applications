using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.DTO;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class GameTypeRepository : BaseRepository<DAL.App.DTO.GameType, Domain.App.GameType, AppDbContext>,
        IGameTypeRepository
    {
        public GameTypeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new GameTypeMapper(mapper))
        {
        }
        
        public override GameType Update(GameType gameType)
        {
            var domainEntity = Mapper.Map(gameType);

            // load the translations (will loose the dal mapper translations)
            domainEntity!.Name = 
                RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.NameId);
            // set the value from dal entity back to list
            domainEntity!.Name.SetTranslation(gameType.Name);
            
            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            var dalEntity = Mapper.Map(updatedEntity);
            return dalEntity!;
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.GameType>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations);
            
            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));
            var res = await resQuery.ToListAsync();
            
            return res!;
        }

        public override async Task<DAL.App.DTO.GameType?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }
    }
}