using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class ReportMapper: BaseMapper<Report, BLL.App.DTO.Report>
    {
        public ReportMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}