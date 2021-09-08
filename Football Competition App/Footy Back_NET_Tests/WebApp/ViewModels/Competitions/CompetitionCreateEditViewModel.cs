using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Competitions
{
    public class CompetitionCreateEditViewModel
    {
        public PublicApi.DTO.v1.Competition Competition { get; set; } = default!;

        public SelectList? CountrySelectList { get; set; }
    }
}