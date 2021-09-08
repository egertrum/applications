using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.CompetitionTeams
{
    public class CompetitionTeamCreateEditViewModel
    {
        public PublicApi.DTO.v1.CompetitionTeam CompetitionTeam { get; set; } = default!;

        public SelectList? CompetitionSelectList { get; set; }
        
        public SelectList? TeamSelectList { get; set; }
        
        public SelectList? CountrySelectList { get; set; }
        
        public Guid? countryId { get; set; }
        public string? Error { get; set; }
    }
}