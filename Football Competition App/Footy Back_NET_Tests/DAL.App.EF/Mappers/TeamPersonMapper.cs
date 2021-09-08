using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class TeamPersonMapper: BaseMapper<DAL.App.DTO.TeamPerson, Domain.App.TeamPerson>
    {
        public TeamPersonMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}