using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class QuizMapper: BaseMapper<Domain.App.Quiz, DTO.App.V1.Quiz>
    {
        public QuizMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}