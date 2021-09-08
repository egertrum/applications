using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class TeamMapper: BaseMapper<Team, BLL.App.DTO.Team>
    {
        public TeamMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}