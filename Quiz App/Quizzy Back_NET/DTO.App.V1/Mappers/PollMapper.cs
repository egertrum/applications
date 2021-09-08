using AutoMapper;
using Domain.App;

namespace DTO.App.V1.Mappers
{
    public class PollMapper: BaseMapper<Domain.App.Poll, DTO.App.V1.Poll>
    {
        public PollMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}