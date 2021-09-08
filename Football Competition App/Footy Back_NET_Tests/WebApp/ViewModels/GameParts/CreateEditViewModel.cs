using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.GameParts
{
    public class CreateEditViewModel
    {
        public PublicApi.DTO.v1.GamePart GamePart { get; set; } = default!;

        public SelectList? GameSelectList { get; set; }
        
        public SelectList? GamePartTypeSelectList { get; set; }
    }
}