using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class GamePartTypeMapper: BaseMapper<GamePartType, BLL.App.DTO.GamePartType>
    {
        public GamePartTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}