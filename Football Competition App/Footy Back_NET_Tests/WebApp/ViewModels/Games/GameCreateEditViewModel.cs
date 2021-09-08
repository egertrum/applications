using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicApi.DTO.v1;

namespace WebApp.ViewModels.Games
{
    public class GameCreateEditViewModel
    {
        public PublicApi.DTO.v1.Game Game { get; set; } = default!;

        public Guid competitionId { get; set; }

        public GameLength GameLength { get; set; } = default!;
        
        public string? ExtraTimeError { get; set; }

        public SelectList? HomeTeamSelectList { get; set; }
        
        public SelectList? AwayTeamSelectList { get; set; }

        public SelectList? GameTypeSelectList { get; set; }
    }
}