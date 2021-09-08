using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class GameTypeMapper: BaseMapper<DAL.App.DTO.GameType, Domain.App.GameType>
    {
        public GameTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}