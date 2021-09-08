using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class UserAnswerMapper: BaseMapper<Domain.App.UserAnswer, DTO.App.V1.UserAnswer>
    {
        public UserAnswerMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}