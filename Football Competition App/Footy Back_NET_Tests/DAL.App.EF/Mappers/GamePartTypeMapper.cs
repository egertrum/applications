using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class GamePartTypeMapper: BaseMapper<DAL.App.DTO.GamePartType, Domain.App.GamePartType>
    {
        public GamePartTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}