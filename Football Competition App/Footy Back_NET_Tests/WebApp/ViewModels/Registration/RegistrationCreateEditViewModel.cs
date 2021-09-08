using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.Registration
{
    public class RegistrationCreateEditViewModel
    {
        public PublicApi.DTO.v1.Registration Registration { get; set; } = default!;

        public SelectList? CompetitionSelectList { get; set; }
        
        public SelectList? TeamSelectList { get; set; }
        
        public SelectList? UserSelectList { get; set; }
    }
}