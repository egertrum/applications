using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class GameTypeMapper: BaseMapper<BLL.App.DTO.GameType, DAL.App.DTO.GameType>
    {
        public GameTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}