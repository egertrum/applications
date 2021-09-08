using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class QuestionAnswerMapper: BaseMapper<Domain.App.QuestionAnswer, DTO.App.V1.QuestionAnswer>
    {
        public QuestionAnswerMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}