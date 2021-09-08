using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicApi.DTO.v1;

namespace WebApp.ViewModels.Teams
{
    public class TeamCreateEditDetailsViewModel
    {
        public PublicApi.DTO.v1.Team Team { get; set; } = default!;

        public bool BelongsToUser { get; set; } = false;
        
        public IEnumerable<TeamPerson>? TeamPersons { get; set; }

        public SelectList? CountrySelectList { get; set; }
    }
}