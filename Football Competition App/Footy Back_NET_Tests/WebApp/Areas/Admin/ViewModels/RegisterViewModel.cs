using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = default!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = default!;
            
        [Required]
        [Display(Name = "First name")]
        [StringLength(128, MinimumLength = 1)]
        public string Firstname { get; set; } = default!;

        [Required]
        [Display(Name = "Last name")]
        [StringLength(128, MinimumLength = 1)]
        public string Lastname { get; set; } = default!;

        [Required]
        [Display(Name = "Identification code")]
        [StringLength(128, MinimumLength = 1)]
        public string IdentificationCode { get; set; } = default!;
            
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birthdate")]
        public DateTime Birthdate { get; set; }
            
        [Display(Name = "Gender")]
        [StringLength(128, MinimumLength = 1)]
        public string? Gender { get; set; }
            
        [Required]
        [Display(Name = "Country")]
        public Guid CountryId { get; set; }

        [Required]
        [Display(Name = "What is your role?")]
        public string Role { get; set; } = default!;
        
        public SelectList? RoleSelectList { get; set; }
        
        public SelectList? CountrySelectList { get; set; }
    }
}