using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class GameTypeMapper: BaseMapper<GameType, BLL.App.DTO.GameType>
    {
        public GameTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}