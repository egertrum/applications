using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class ParticipationMapper: BaseMapper<DAL.App.DTO.Participation, Domain.App.Participation>
    {
        public ParticipationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}