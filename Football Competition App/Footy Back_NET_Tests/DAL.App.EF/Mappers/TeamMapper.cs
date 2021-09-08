using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class TeamMapper: BaseMapper<DAL.App.DTO.Team, Domain.App.Team>
    {
        public TeamMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}