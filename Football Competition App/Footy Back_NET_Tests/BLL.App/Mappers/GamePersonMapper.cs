using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class GamePersonMapper: BaseMapper<BLL.App.DTO.GamePerson, DAL.App.DTO.GamePerson>
    {
        public GamePersonMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}