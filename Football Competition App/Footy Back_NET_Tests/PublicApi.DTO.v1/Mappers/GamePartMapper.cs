using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class GamePartMapper: BaseMapper<GamePart, BLL.App.DTO.GamePart>
    {
        public GamePartMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}