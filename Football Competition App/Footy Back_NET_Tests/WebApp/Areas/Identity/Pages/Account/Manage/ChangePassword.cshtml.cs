using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Base.Resources;
using Base.Resources.Areas.Identity.Pages.Account.Manage;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public ChangePasswordModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        [TempData]
        public string StatusMessage { get; set; }  = default!;

        public PasswordRequirementsViewModel? PasswordRequirements { get; set; }
        
        public class PasswordRequirementsViewModel
        {
            public bool RequireDigit { get; set; }
            public int RequiredLength { get; set; }
            public bool RequireLowercase { get; set; }
            public bool RequireUppercase { get; set; }
            public int RequiredUniqueChars { get; set; }
            public bool RequireNonAlphanumeric { get; set; }
        }

        public class InputModel
        {

            [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Common))]
            [DataType(DataType.Password)]
            [Display(Name = nameof(OldPassword), ResourceType = typeof(ChangePassword))]
            public string OldPassword { get; set; } = default!;
            

            [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Common))]
            [StringLength(100, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Common), MinimumLength = 6)]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = nameof(NewPassword), ResourceType = typeof(ChangePassword))]
            public string NewPassword { get; set; } = default!;
            
            [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Common))]
            [DataType(DataType.Password)]
            [Display(Name = nameof(ConfirmPassword), ResourceType = typeof(ChangePassword))]
            [Compare("NewPassword", ErrorMessageResourceName = "ErrorMessage_ComparePassword", ErrorMessageResourceType = typeof(Common))]
            public string ConfirmPassword { get; set; } = default!;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }
            PasswordRequirements = new PasswordRequirementsViewModel()
            {
                RequireDigit = _userManager.Options.Password.RequireDigit,
                RequiredLength = _userManager.Options.Password.RequiredLength,
                RequireLowercase = _userManager.Options.Password.RequireLowercase,
                RequireUppercase = _userManager.Options.Password.RequireUppercase,
                RequiredUniqueChars = _userManager.Options.Password.RequiredUniqueChars,
                RequireNonAlphanumeric = _userManager.Options.Password.RequireNonAlphanumeric
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully");
            StatusMessage = "Your password has been changed.";

            return RedirectToPage();
        }
    }
}