using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class GamePartMapper: BaseMapper<BLL.App.DTO.GamePart, DAL.App.DTO.GamePart>
    {
        public GamePartMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}