using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App.Identity;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        private readonly IAppBLL _bll;

        public UsersController(UserManager<AppUser> userManager, IAppBLL bll, AppDbContext context)
        {
            _userManager = userManager;
            _bll = bll;
            _context = context;
        }

        public class UsersEditModel
        {
            [BindProperty] public string UserRole { get; set; } = default!;
            
            [BindProperty]
            [Required]
            [Display(Name = "First name")]
            [StringLength(128, MinimumLength = 1)]
            public string Firstname { get; set; } = default!;

            [BindProperty]
            [Required]
            [Display(Name = "Last name")]
            [StringLength(128, MinimumLength = 1)]
            public string Lastname { get; set; } = default!;
        }

        // GET: Admin/Roles
        public async Task<IActionResult> Index()
        {
            var vm = new UserIndexViewModel();
            var res = await _context.Users.ToListAsync();
            var dtoUsers = new List<PublicApi.DTO.v1.AppUser>();
            
            foreach (var user in res)
            {
                var dtoUser = new PublicApi.DTO.v1.AppUser()
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    UserName = user.UserName,
                    Email = user.Email,
                    IdentificationCode = user.IdentificationCode,
                    Id = user.Id,
                    AppRoles = await _userManager.GetRolesAsync(user)
                };
                dtoUsers.Add(dtoUser);
            }

            vm.AppUsers = dtoUsers;
            return View(vm);
        }

        // GET: Admin/Roles/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Admin/Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Admin/Roles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: Admin/Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id)
        {
            var appUser = await _context.Users.FindAsync(id);
            
            if (id != appUser.Id)
            {
                return NotFound();
            }
            
            //await _userManager.AddToRoleAsync(appUser, "TeamManager");
            
            //TODO vaata bindimine yle controlleri ja view vahel
            //ViewData["UserRoles"] = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");

            if (ModelState.IsValid)
            {
                /*
                var oldRole = await _userManager.GetRolesAsync(appUser);

                foreach (var roleName in oldRole)
                {
                    var result = _userManager.RemoveFromRoleAsync(appUser, roleName).Result;
                    if (!result.Succeeded)
                    {
                        foreach (var identityError in result.Errors)
                        {
                            Console.WriteLine("Cant add user to role! Error: " + identityError.Description);
                        }
                    }   
                }
                */

                //await _userManager.AddToRoleAsync(appUser, "TeamManager");
                
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }

        // GET: Admin/Roles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: Admin/Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var appUser = await _context.Users.FindAsync(id);
            var persons = await _bll.Persons.GetAllAsync(appUser.Id);
            var teams = await _bll.Teams.GetAllAsync(appUser.Id);
            var registrations = await _bll.Registrations.GetAllAsync(appUser.Id);
            foreach (var person in persons)
            {
                person.AppUserId = User.GetUserId()!.Value;
                _bll.Persons.Update(person);
            }
            foreach (var team in teams)
            {
                team.AppUserId = User.GetUserId()!.Value;
                _bll.Teams.Update(team);
            }
            foreach (var registration in registrations)
            {
                registration.AppUserId = User.GetUserId()!.Value;
                _bll.Registrations.Update(registration);
            }
            await _bll.SaveChangesAsync();
            //await _userManager.RemoveFromRoleAsync(appUser, "FootyUser");
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
