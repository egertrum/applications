using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class GameLengthMapper: BaseMapper<GameLength, BLL.App.DTO.GameLength>
    {
        public GameLengthMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}