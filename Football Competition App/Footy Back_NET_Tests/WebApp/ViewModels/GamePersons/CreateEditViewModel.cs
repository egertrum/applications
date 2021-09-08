using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.GamePersons
{
    public class CreateEditViewModel
    {
        public PublicApi.DTO.v1.GamePerson GamePerson { get; set; } = default!;

        public SelectList? PersonSelectList { get; set; }
        
    }
}