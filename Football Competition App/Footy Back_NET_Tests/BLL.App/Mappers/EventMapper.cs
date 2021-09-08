using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class EventMapper: BaseMapper<BLL.App.DTO.Event, DAL.App.DTO.Event>
    {
        public EventMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}