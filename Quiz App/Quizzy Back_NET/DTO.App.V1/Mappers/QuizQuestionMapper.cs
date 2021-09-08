using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class QuizQuestionMapper: BaseMapper<Domain.App.QuizQuestion, DTO.App.V1.QuizQuestion>
    {
        public QuizQuestionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}