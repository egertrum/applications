using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class ParticipationMapper: BaseMapper<BLL.App.DTO.Participation, DAL.App.DTO.Participation>
    {
        public ParticipationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}