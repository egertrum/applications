using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class CompetitionTeamMapper : BaseMapper<CompetitionTeam, BLL.App.DTO.CompetitionTeam>
    {
        public CompetitionTeamMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}