using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class QuestionMapper: BaseMapper<Domain.App.Question, DTO.App.V1.Question>
    {
        public QuestionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}