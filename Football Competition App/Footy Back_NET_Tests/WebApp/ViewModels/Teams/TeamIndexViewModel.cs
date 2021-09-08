using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicApi.DTO.v1;

namespace WebApp.ViewModels.Teams
{
    public class TeamIndexViewModel
    {
        /// <summary>
        /// List of Teams
        /// </summary>
        public IEnumerable<Team> Teams { get; set; } = default!;

        /// <summary>
        /// If true then view accordingly(added possibility to delete and edit competitions) and vice versa.
        /// </summary>
        public bool UserTeams { get; set; }
        
        public string? Title { get; set; }
        
        public string? Message { get; set; }
    }
}