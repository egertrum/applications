using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class TeamPersonMapper: BaseMapper<BLL.App.DTO.TeamPerson, DAL.App.DTO.TeamPerson>
    {
        public TeamPersonMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}