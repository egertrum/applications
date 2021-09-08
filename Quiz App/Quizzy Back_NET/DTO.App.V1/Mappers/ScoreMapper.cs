using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class ScoreMapper: BaseMapper<Domain.App.Score, DTO.App.V1.Score>
    {
        public ScoreMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}