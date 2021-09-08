using System.Collections.Generic;

namespace WebApp.Areas.Admin.ViewModels
{
    public class UserIndexViewModel
    {
        public IEnumerable<PublicApi.DTO.v1.AppUser> AppUsers { get; set; } = default!;
        
    }
}