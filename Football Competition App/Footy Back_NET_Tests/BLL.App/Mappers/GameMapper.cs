using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class GameMapper: BaseMapper<BLL.App.DTO.Game, DAL.App.DTO.Game>
    {
        public GameMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}