using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty] public InputModel Input { get; set; } = default!;

        public PasswordRequirementsViewModel? PasswordRequirements { get; set; }

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; } = default!;

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
            [Required(ErrorMessageResourceType = typeof(Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [EmailAddress(ErrorMessageResourceType = typeof(Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Email")]
            [Display(ResourceType = typeof(Resources.Areas.Identity.Pages.Account.Register),
                Name = nameof(Email))]
            public string Email { get; set; } = default!;

            [Required(ErrorMessageResourceType = typeof(Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_Required")]
            [StringLength(100, ErrorMessageResourceType = typeof(Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = nameof(Password),
                ResourceType = typeof(Resources.Areas.Identity.Pages.Account.Register))]
            public string Password { get; set; } = default!;

            [DataType(DataType.Password)]
            [Display(Name = nameof(ConfirmPassword),
                ResourceType = typeof(Resources.Areas.Identity.Pages.Account.Register))]
            [Compare("Password",
                ErrorMessageResourceType = typeof(Resources.Areas.Identity.Pages.Account.Register),
                ErrorMessageResourceName = "PasswordsDontMatch")]
            public string ConfirmPassword { get; set; } = default!;

            [MaxLength(128, ErrorMessageResourceType = typeof(Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")] 
            [Display(ResourceType = typeof(Resources.Areas.Identity.Pages.Account.Register),
                Name = nameof(FirstName))]
            public virtual string FirstName { get; set; } = default!;

            [MaxLength(128, ErrorMessageResourceType = typeof(Resources.Common),
                ErrorMessageResourceName = "ErrorMessage_MaxLength")] 
            [Display(ResourceType = typeof(Resources.Areas.Identity.Pages.Account.Register),
                Name = nameof(LastName))]
            public virtual string LastName { get; set; } = default!;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            PasswordRequirements = new PasswordRequirementsViewModel()
            {
                RequireDigit = _userManager.Options.Password.RequireDigit,
                RequiredLength = _userManager.Options.Password.RequiredLength,
                RequireLowercase = _userManager.Options.Password.RequireLowercase,
                RequireUppercase = _userManager.Options.Password.RequireUppercase,
                RequiredUniqueChars = _userManager.Options.Password.RequiredUniqueChars,
                RequireNonAlphanumeric = _userManager.Options.Password.RequireNonAlphanumeric
            };
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User created a new account with password");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new {area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl},
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl = returnUrl});
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return BadRequest();
        }
    }
}