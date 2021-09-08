using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class GamePartTypeMapper: BaseMapper<BLL.App.DTO.GamePartType, DAL.App.DTO.GamePartType>
    {
        public GamePartTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}