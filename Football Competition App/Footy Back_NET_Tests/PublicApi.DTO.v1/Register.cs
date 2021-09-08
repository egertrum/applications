using System;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.DTO.v1
{
    public class Register
    {
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [EmailAddress(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Email")]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = nameof(Email))]
        public string Email { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [StringLength(100, ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = nameof(Password),
            ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register))]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(ConfirmPassword),
            ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register))]
        [Compare("Password",
            ErrorMessageResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            ErrorMessageResourceName = "PasswordsDontMatch")]
        public string ConfirmPassword { get; set; } = default!;

        [MaxLength(128, ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = nameof(FirstName))]
        public virtual string FirstName { get; set; } = default!;

        [MaxLength(128, ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_MaxLength")]
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = nameof(LastName))]
        public virtual string LastName { get; set; } = default!;

        [StringLength(128, MinimumLength = 1)]
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = nameof(IdentificationCode))]
        public string IdentificationCode { get; set; } = default!;

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = "Birthdate")]
        public DateTime BirthDate { get; set; }


        [StringLength(128, MinimumLength = 1)]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = nameof(Gender))]
        public string? Gender { get; set; }

        [Required(ErrorMessageResourceType = typeof(Base.Resources.Common),
            ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(ResourceType = typeof(Base.Resources.Areas.Identity.Pages.Account.Register),
            Name = nameof(Country))]
        public Guid CountryId { get; set; }
    }
}