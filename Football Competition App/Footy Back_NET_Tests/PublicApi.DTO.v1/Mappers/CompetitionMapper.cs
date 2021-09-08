using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class CompetitionMapper : BaseMapper<Competition, BLL.App.DTO.Competition>
    {
        public CompetitionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}