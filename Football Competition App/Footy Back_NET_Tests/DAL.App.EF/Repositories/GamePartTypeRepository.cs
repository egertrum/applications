using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Domain.App;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using GamePartType = DAL.App.DTO.GamePartType;

namespace DAL.App.EF.Repositories
{
    public class GamePartTypeRepository: BaseRepository<DAL.App.DTO.GamePartType, Domain.App.GamePartType, AppDbContext>, IGamePartTypeRepository
    {
        public GamePartTypeRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new GamePartTypeMapper(mapper))
        {
        }

        public override GamePartType Update(DTO.GamePartType gamePartType)
        {
            var domainEntity = Mapper.Map(gamePartType);

            // load the translations (will loose the dal mapper translations)
            domainEntity!.Name = 
                RepoDbContext.LangStrings
                    .Include(t => t.Translations)
                    .First(x => x.Id == domainEntity.NameId);
            // set the value from dal entity back to list
            domainEntity!.Name.SetTranslation(gamePartType.Name);
            domainEntity.Short = gamePartType.Short;
            
            var updatedEntity = RepoDbSet.Update(domainEntity!).Entity;
            var dalEntity = Mapper.Map(updatedEntity);
            return dalEntity!;
        }
        
        public override async Task<IEnumerable<DAL.App.DTO.GamePartType>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();
            
            return res!;
        }
        
        public override async Task<DAL.App.DTO.GamePartType?> FirstOrDefaultAsync(Guid id, Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query
                .Include(c => c.Name)
                .ThenInclude(t => t!.Translations);

            var res = Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));

            return res;
        }

        public async Task<Guid> FindIdByShort(EGamePartType shortName)
        {
            var query = CreateQuery();

            var res = await query.FirstOrDefaultAsync(g => g.Short == shortName);

            return res.Id;
        }
    }
}