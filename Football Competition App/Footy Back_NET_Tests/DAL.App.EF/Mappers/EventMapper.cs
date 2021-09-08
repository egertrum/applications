using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class EventMapper: BaseMapper<DAL.App.DTO.Event, Domain.App.Event>
    {
        public EventMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}