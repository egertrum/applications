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
using WebApp.ViewModels.Registration;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RegistrationsController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.RegistrationMapper _registrationMapper;

        public RegistrationsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _registrationMapper = new RegistrationMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.Registrations.GetAllAsync()).Select(x => _registrationMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var registration = _registrationMapper.Map(await _bll.Registrations.FirstOrDefaultAsync(id.Value));
            if (registration == null) return NotFound();

            return View(registration);
        }
        
        public async Task<IActionResult> Create()
        {
            var vm = new RegistrationCreateEditViewModel()
            {
                CompetitionSelectList = new SelectList(await _bll.Competitions.GetAllAsync(), "Id", "Name"),
                TeamSelectList = new SelectList(await _bll.Teams.GetAllAsync(), "Id", "Name"),
                UserSelectList = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "Name")
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegistrationCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Registrations.Add(_registrationMapper.Map(vm.Registration)!);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.CompetitionSelectList = new SelectList(await _bll.Competitions.GetAllAsync(), "Id", "Name");
            vm.TeamSelectList = new SelectList(await _bll.Teams.GetAllAsync(), "Id", "Name");
            vm.UserSelectList = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "Name");
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var registration = _registrationMapper.Map(await _bll.Registrations.FirstOrDefaultAsync(id.Value));
            if (registration == null) return NotFound();
            var vm = new RegistrationCreateEditViewModel()
            {
                Registration = registration,
                CompetitionSelectList = new SelectList(await _bll.Competitions.GetAllAsync(), "Id", "Name"),
                TeamSelectList = new SelectList(await _bll.Teams.GetAllAsync(), "Id", "Name"),
                UserSelectList = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "Name")
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, RegistrationCreateEditViewModel vm)
        {
            if (id != vm.Registration.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                vm.CompetitionSelectList = new SelectList(await _bll.Competitions.GetAllAsync(), "Id", "Name");
                vm.TeamSelectList = new SelectList(await _bll.Teams.GetAllAsync(), "Id", "Name");
                vm.UserSelectList = new SelectList(await _bll.AppUsers.GetAllAsync(), "Id", "Name");
                return View(vm);
            }

            var bllRegistration = _registrationMapper.Map(vm.Registration);
            _bll.Registrations.Update(bllRegistration!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var registration = _registrationMapper.Map(await _bll.Registrations.FirstOrDefaultAsync(id.Value));
            if (registration == null) return NotFound();

            return View(registration);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Registrations.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
