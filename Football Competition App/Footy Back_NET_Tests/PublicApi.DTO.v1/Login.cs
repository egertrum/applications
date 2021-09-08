using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class Login
    {
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Email")]
        [Display(Name = nameof(Email), ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Login))] 
        public string Email { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common), ErrorMessageResourceName = "ErrorMessage_Required")]

        [DataType(DataType.Password)]
        [Display(Name = nameof(Password), ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Login))] 
        public string Password { get; set; } = default!;

        [Display(Name = nameof(RememberMe), ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Login))] 
        public bool RememberMe { get; set; }
    }
}