using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class TeamMapper: BaseMapper<BLL.App.DTO.Team, DAL.App.DTO.Team>
    {
        public TeamMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}