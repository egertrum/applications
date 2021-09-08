using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class CompetitionTeamMapper: BaseMapper<DAL.App.DTO.CompetitionTeam, Domain.App.CompetitionTeam>
    {
        public CompetitionTeamMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}