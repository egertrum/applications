using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Resources;
using DAL.App.EF;
using Domain.App.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Index = Resources.Areas.Identity.Pages.Account.Manage.Index;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _ctx;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, 
            AppDbContext ctx)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ctx = ctx;
        }

        [Display(Name = nameof(Username), ResourceType = typeof(Index))]
        public string? Username { get; set; }


        [TempData] public string StatusMessage { get; set; } = default!;

        [BindProperty] public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Common))]
            [Phone(ErrorMessageResourceName = "ErrorMessage_NotValidPhone", ErrorMessageResourceType = typeof(Common))]
            [Display(Name = nameof(PhoneNumber), ResourceType = typeof(Index))]
            public string? PhoneNumber { get; set; }

            [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Common))]
            [StringLength(128, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Common), MinimumLength = 1)]
            [Display(Name = nameof(FirstName), ResourceType = typeof(Index))]
            public string FirstName { get; set; } = default!;
            
            [Required(ErrorMessageResourceName = "ErrorMessage_Required", ErrorMessageResourceType = typeof(Common))]
            [StringLength(128, ErrorMessageResourceName = "ErrorMessage_StringLengthMinMax", ErrorMessageResourceType = typeof(Common), MinimumLength = 1)]
            [Display(Name = nameof(LastName), ResourceType = typeof(Index))]
            public string LastName { get; set; } = default!;
            
        }

        private async Task LoadAsync(AppUser user)
        {
            
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? "",
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(string.Format(Index.Unable_to_load_user_with_ID,_userManager.GetUserId(User)));
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(string.Format(Index.Unable_to_load_user_with_ID,_userManager.GetUserId(User)));
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = Index.Unexpected_error_when_trying_to_set_phone_number;
                    return RedirectToPage();
                }
            }


            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;

          
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                StatusMessage = Index.Unexpected_error_when_trying_to_update_profile_data;
                return RedirectToPage();
            }
            
            
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = Index.Your_profile_has_been_updated;

            return RedirectToPage();
        }
    }
}