using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class PollQuestionMapper: BaseMapper<Domain.App.PollQuestion, DTO.App.V1.PollQuestion>
    {
        public PollQuestionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}