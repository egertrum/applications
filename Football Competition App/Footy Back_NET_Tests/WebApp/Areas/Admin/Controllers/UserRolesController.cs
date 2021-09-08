using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUnitOfWork _uow;

        public UserRolesController(AppDbContext context, UserManager<AppUser> userManager, IAppUnitOfWork uow)
        {
            _context = context;
            _userManager = userManager;
            _uow = uow;
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
            
            return View(await _context.UserRoles.ToListAsync());
        }

        // GET: Admin/Roles/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: Admin/Roles/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserRoles"] = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");
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
                appUser.Id = Guid.NewGuid();
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
            
            //ViewData["UserRoles"] = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");

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
        public async Task<IActionResult> Edit(Guid id, AppUser appUser, string userRole)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }
            
            //TODO vaata bindimine yle controlleri ja view vahel
            //ViewData["UserRoles"] = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name");

            if (ModelState.IsValid)
            {
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

                await _userManager.AddToRoleAsync(appUser, userRole);
                
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
            var persons = await _uow.Persons.GetAllAsync(appUser.Id);
            foreach (var person in persons)
            {
                person.AppUserId = User.GetUserId()!.Value;
            }
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppRoleExists(Guid id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
