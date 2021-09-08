using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App.Repositories;
using DAL.App.EF.Mappers;
using DAL.Base.EF.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Repositories
{
    public class ReportRepository: BaseRepository<DAL.App.DTO.Report, Domain.App.Report, AppDbContext>, IReportRepository
    {
        public ReportRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext, new ReportMapper(mapper))
        {
        }

        public override async Task<IEnumerable<DAL.App.DTO.Report>> GetAllAsync(Guid userId = default, bool noTracking = true)
        {
            var query = CreateQuery();

            query = query.OrderBy(x => x.Date);

            var resQuery = query.Select(domainEntity => Mapper.Map(domainEntity));

            var res = await resQuery.ToListAsync();
            
            return res!;
        }
    }
}