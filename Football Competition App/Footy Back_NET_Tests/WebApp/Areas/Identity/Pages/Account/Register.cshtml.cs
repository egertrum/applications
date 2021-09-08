using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Contracts.DAL.App;
using DAL.App.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PublicApi.DTO.v1;
using AppUser = Domain.App.Identity.AppUser;
using Person = Domain.App.Person;

namespace WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IAppUnitOfWork _uow;
        private readonly AppDbContext _ctx;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal.RegisterModel> logger,
            IEmailSender emailSender,
            IAppUnitOfWork uow, AppDbContext ctx)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _uow = uow;
            _ctx = ctx;
        }

        [BindProperty] public Register Input { get; set; } = default!;

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

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            ViewData["Countries"] = new SelectList(await _uow.Countries.GetAllAsync(), "Id", "Name");

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

            if (_ctx!.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                if (ModelState.ContainsKey("Input.BirthDate"))
                    ModelState["Input.BirthDate"].ValidationState = ModelValidationState.Valid;
                Input.BirthDate = DateTime.Now.Date;
            }
            

            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = Input.Email, Email = Input.Email, Firstname = Input.FirstName, 
                    Lastname = Input.LastName, IdentificationCode = Input.IdentificationCode};
                
                var person = new Person { AppUser  = user, AppUserId = user.Id, CountryId = Input.CountryId,
                    BirthDate = Input.BirthDate, FirstName = user.Firstname, LastName = user.Lastname, 
                    IdentificationCode = Input.IdentificationCode, Gender = Input.Gender};
                
                ViewData["Countries"] = new SelectList(await _uow.Countries.GetAllAsync(), "Id", "Name", person.CountryId);
                
                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, "FootyUser");
                if (result.Succeeded)
                {
                    await _ctx.Persons.AddAsync(person);
                    await _uow.SaveChangesAsync();
                    _logger.LogInformation("User created a new account with password.");

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