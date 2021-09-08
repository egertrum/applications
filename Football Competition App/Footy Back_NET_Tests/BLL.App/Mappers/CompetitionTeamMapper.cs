using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class CompetitionTeamMapper: BaseMapper<BLL.App.DTO.CompetitionTeam, DAL.App.DTO.CompetitionTeam>
    {
        public CompetitionTeamMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}