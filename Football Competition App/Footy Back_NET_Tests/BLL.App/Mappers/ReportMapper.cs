using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class ReportMapper: BaseMapper<BLL.App.DTO.Report, DAL.App.DTO.Report>
    {
        public ReportMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}