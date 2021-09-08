using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class GameMapper: BaseMapper<DAL.App.DTO.Game, Domain.App.Game>
    {
        public GameMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}