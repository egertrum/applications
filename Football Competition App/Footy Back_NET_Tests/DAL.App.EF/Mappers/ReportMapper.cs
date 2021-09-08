using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class ReportMapper: BaseMapper<DAL.App.DTO.Report, Domain.App.Report>
    {
        public ReportMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}