using System.Collections.Generic;

namespace WebApp.ViewModels.Competitions
{
    public class CompetitionDetailsViewModel
    {
        public PublicApi.DTO.v1.Competition Competition { get; set; } = default!;
        
        public IEnumerable<PublicApi.DTO.v1.CompetitionTeam>? CompetitionTeams { get; set; }
        
        public IEnumerable<PublicApi.DTO.v1.Game>? Games { get; set; }
    }
}