using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain.App;
using Domain.App.Identity;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WebApp.Areas.Identity.Pages.Account;

namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    /// API Controller for Account management.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _ctx;

        
        /// <summary>
        /// Constructor for Account API Controller.
        /// </summary>
        /// <param name="signInManager">To manage sign in.</param>
        /// <param name="userManager">For user management.</param>
        /// <param name="logger">Logger</param>
        /// <param name="configuration">Configuration</param>
        /// <param name="ctx">Database context</param>
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            ILogger<AccountController> logger, IConfiguration configuration, AppDbContext ctx)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
            _ctx = ctx;
        }

        /// <summary>
        /// Logging in.
        /// </summary>
        /// <param name="dto">PublicApi version 1 Data Transfer Object of Login entity type</param>
        /// <returns>JWT response if everything successful - user token, firstname, lastname and role.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.JwtResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> Login([FromBody] PublicApi.DTO.v1.Login dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            
            if (appUser == null)
            {
                _logger.LogWarning("WebApi login failed. User {User} not found", dto.Email);
                return NotFound(new PublicApi.DTO.v1.Message("User/Password problem!"));
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, false);
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
                var jwt = Extensions.Base.IdentityExtensions.GenerateJwt(
                    claimsPrincipal.Claims,
                    _configuration["JWT:Key"],                    
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Issuer"],
                    // _configuration.GetValue<int> - for casting
                    DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                _logger.LogInformation("WebApi login. User {User}", dto.Email);
                var roles = await _userManager.GetRolesAsync(appUser);
                return Ok(new PublicApi.DTO.v1.JwtResponse()
                {
                    Token = jwt,
                    Firstname = appUser.Firstname,
                    Lastname = appUser.Lastname,
                    Role = roles[0]
                });
            }
            
            _logger.LogWarning("WebApi login failed. User {User} - bad password", dto.Email);
            return NotFound(new PublicApi.DTO.v1.Message("User/Password problem!"));
        }

        /// <summary>
        /// Registering.
        /// </summary>
        /// <param name="dto">PublicApi version 1 Data Transfer Object of Register entity type</param>
        /// <returns>JWT response if everything successful - user token, firstname, lastname and role.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.JwtResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Register([FromBody] PublicApi.DTO.v1.Register dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            if (appUser != null)
            {
                _logger.LogWarning(" User {User} already registered", dto.Email);
                return BadRequest(new PublicApi.DTO.v1.Message("User already registered"));
            }

            appUser = new Domain.App.Identity.AppUser()
            {
                Email = dto.Email,
                UserName = dto.Email,
                Firstname = dto.FirstName,
                Lastname = dto.LastName,
                IdentificationCode = dto.IdentificationCode
            };
            var result = await _userManager.CreateAsync(appUser, dto.Password);
            await _userManager.AddToRoleAsync(appUser, "FootyUser");
            
            var person = new Person { AppUser  = appUser, AppUserId = appUser.Id, CountryId = dto.CountryId,
                BirthDate = DateTime.Now, FirstName = dto.FirstName, LastName = dto.LastName, 
                IdentificationCode = dto.IdentificationCode, Gender = dto.Gender};
                
            if (result.Succeeded)
            {
                await _ctx.Persons.AddAsync(person);
                await _ctx.SaveChangesAsync();
                _logger.LogInformation("User {Email} created a new account with password", appUser.Email);
                
                var user = await _userManager.FindByEmailAsync(appUser.Email);
                if (user != null)
                {                
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var jwt = Extensions.Base.IdentityExtensions.GenerateJwt(
                        claimsPrincipal.Claims,
                        _configuration["JWT:Key"],                    
                        _configuration["JWT:Issuer"],
                        _configuration["JWT:Issuer"],
                        DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                    var roles = await _userManager.GetRolesAsync(appUser);
                    _logger.LogInformation("WebApi login. User {User}", dto.Email);
                    return Ok(new PublicApi.DTO.v1.JwtResponse()
                    {
                        Token = jwt,
                        Firstname = appUser.Firstname,
                        Lastname = appUser.Lastname,
                        Role = roles[0]
                    });
                }
                _logger.LogInformation("User {Email} not found after creation", appUser.Email);
                return BadRequest(new PublicApi.DTO.v1.Message("User not found after creation!"));
            }
            var errors = result.Errors.Select(error => error.Description).ToList();
            return BadRequest(new PublicApi.DTO.v1.Message() {Messages = errors});
        }
        
        
        /// <summary>
        /// Get currently logged in user information to set up a profile view.
        /// </summary>
        /// <returns>PublicApi version 1 Data Transfer Object of AppUser entity type</returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.AppUser), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.AppUser>> GetUserInfo()
        {
            var user = await _userManager.FindByIdAsync(User.GetUserId()!.Value.ToString());
            if (user == null) return NotFound();

            var dtoUser = new PublicApi.DTO.v1.AppUser()
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                IdentificationCode = user.IdentificationCode
            };
            return dtoUser;
        }
    }
}
