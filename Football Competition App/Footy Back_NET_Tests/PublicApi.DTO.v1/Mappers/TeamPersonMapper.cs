using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class TeamPersonMapper: BaseMapper<TeamPerson, BLL.App.DTO.TeamPerson>
    {
        public TeamPersonMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}