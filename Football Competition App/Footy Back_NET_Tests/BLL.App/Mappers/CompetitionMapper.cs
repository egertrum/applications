using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class CompetitionMapper: BaseMapper<BLL.App.DTO.Competition, DAL.App.DTO.Competition>
    {
        public CompetitionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}