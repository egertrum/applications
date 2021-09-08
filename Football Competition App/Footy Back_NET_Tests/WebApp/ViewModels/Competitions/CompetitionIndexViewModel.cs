using System.Collections.Generic;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicApi.DTO.v1;
using Competition = PublicApi.DTO.v1.Competition;

namespace WebApp.ViewModels.Competitions
{
    public class CompetitionIndexViewModel
    {
        /// <summary>
        /// List of Competitions
        /// </summary>
        public IEnumerable<Competition> Competitions { get; set; } = default!;
        
        /// <summary>
        /// SelectList for all of the countries
        /// </summary>
        public SelectList CountrySelectList { get; set; } = default!;
        
        /// <summary>
        /// Competition with search parameters
        /// </summary>
        public SearchCompetition SearchCompetition { get; set; } = default!;

        /// <summary>
        /// If true then view accordingly(added possibility to delete and edit competitions) and vice versa.
        /// </summary>
        public bool UserCompetitions { get; set; }
        
        /// <summary>
        /// To generate some sort of error messaging(i.e somehow trying to delete someone else's competition)
        /// </summary>
        public string? Error { get; set; }
        
        /// <summary>
        /// To generate a certain message.
        /// </summary>
        public string? Message { get; set; }
        
        public string? Title { get; set; }
    }
}