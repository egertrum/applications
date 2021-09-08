using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class CompetitionMapper: BaseMapper<DAL.App.DTO.Competition, Domain.App.Competition>
    {
        public CompetitionMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}