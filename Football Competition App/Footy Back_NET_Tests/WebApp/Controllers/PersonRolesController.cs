using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonRolesController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.RoleMapper _personRoleMapper;

        public PersonRolesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _personRoleMapper = new RoleMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.Roles.GetAllAsync()).Select(x => _personRoleMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var role = _personRoleMapper.Map(await _bll.Roles.FirstOrDefaultAsync(id.Value));
            if (role == null) return NotFound();

            return View(role);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublicApi.DTO.v1.Role role)
        {
            if (!ModelState.IsValid) return View(role);
            _bll.Roles.Add(_personRoleMapper.Map(role)!);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var role = _personRoleMapper.Map(await _bll.Roles.FirstOrDefaultAsync(id.Value));
            if (role == null) return NotFound();
            
            return View(role);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PublicApi.DTO.v1.Role role)
        {
            if (id != role.Id) return NotFound();

            if (!ModelState.IsValid) return View(role);

            var bllRole = _personRoleMapper.Map(role);
            _bll.Roles.Update(bllRole!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var role = _personRoleMapper.Map(await _bll.Roles.FirstOrDefaultAsync(id.Value));
            if (role == null) return NotFound();

            return View(role);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Roles.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
