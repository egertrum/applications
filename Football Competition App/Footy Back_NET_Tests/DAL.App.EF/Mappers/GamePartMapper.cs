using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class GamePartMapper: BaseMapper<DAL.App.DTO.GamePart, Domain.App.GamePart>
    {
        public GamePartMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}