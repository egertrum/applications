using System.Collections.Generic;
using PublicApi.DTO.v1;

namespace WebApp.ViewModels.CompetitionTeams
{
    public class CompetitionTeamIndexModel
    {
        public IEnumerable<CompetitionTeam> CompetitionTeams { get; set; } = default!;

        public bool UserRegisters { get; set; }
        
        public string? Message { get; set; }
    }
}