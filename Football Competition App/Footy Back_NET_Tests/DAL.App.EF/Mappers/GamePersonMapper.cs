using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class GamePersonMapper: BaseMapper<DAL.App.DTO.GamePerson, Domain.App.GamePerson>
    {
        public GamePersonMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}