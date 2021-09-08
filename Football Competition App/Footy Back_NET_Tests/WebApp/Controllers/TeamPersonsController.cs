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
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1.Mappers;
using WebApp.ViewModels.TeamPersons;

namespace WebApp.Controllers
{
    [Authorize]
    public class TeamPersonsController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.TeamPersonMapper _teamPersonMapper;

        public TeamPersonsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _teamPersonMapper = new TeamPersonMapper(mapper);
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.TeamPersons.GetAllAsync())
                .Select(x => _teamPersonMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var teamPerson = _teamPersonMapper.Map(await _bll.TeamPersons.FirstOrDefaultAsync(id.Value));
            if (teamPerson == null) return NotFound();

            return View(teamPerson);
        }
        
        public async Task<IActionResult> Create(Guid? id, Guid? personId)
        {
            var vm = new TeamPersonCreateEditViewModel();
            vm = await BindSelectListsToViewModel(vm, personId, id, null);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamPersonCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var bllTeamPerson = _teamPersonMapper.Map(vm.TeamPerson);
                var allowed = await _bll.TeamPersons.TeamAndPersonBelongToUser(bllTeamPerson!, User.GetUserId()!.Value);
                if (!allowed)
                    return RedirectToAction("Index", "Competitions",
                        new {error = true});
                var addedPerson = await _bll.TeamPersons.AddIfPossible(bllTeamPerson!);
                if (addedPerson != null)
                    return RedirectToAction("MyIndex", "Teams");

                vm.Error = Base.Resources.DTO.v1.TeamPerson.AlreadyExistingMember;
            }

            vm = await BindSelectListsToViewModel(vm, vm.TeamPerson.PersonId, vm.TeamPerson.TeamId,
                vm.TeamPerson.RoleId);
            return View(vm);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var teamPerson = _teamPersonMapper.Map(await _bll.TeamPersons.FirstOrDefaultAsync(id.Value));
            if (teamPerson == null) return NotFound();

            return View(teamPerson);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var bllTeamPerson = await _bll.TeamPersons.FirstOrDefaultAsync(id);
            
            var allowed = await _bll.TeamPersons.TeamAndPersonBelongToUser(bllTeamPerson!, User.GetUserId()!.Value);
            if (!allowed && !User.IsInRole("Admin"))
                return RedirectToAction("Index", "Competitions",
                    new {error = true});
            
            await _bll.TeamPersons.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction("Details", "Teams", new {id = bllTeamPerson!.TeamId});
        }

        private async Task<TeamPersonCreateEditViewModel> BindSelectListsToViewModel(TeamPersonCreateEditViewModel vm,
            Guid? personId,
            Guid? teamId, Guid? roleId)
        {
            vm.PersonSelectList = new SelectList(await _bll.Persons.GetAllAsync(User.GetUserId()!.Value),
                nameof(Person.Id),
                nameof(Person.Name));
            vm.TeamSelectList = new SelectList(await _bll.Teams.GetAllAsync(User.GetUserId()!.Value), nameof(Team.Id),
                nameof(Team.Name));
            vm.RoleSelectList = new SelectList(await _bll.Roles.GetAllAsync(), nameof(Role.Id),
                nameof(Role.Name));

            if (personId != null && personId != Guid.Empty)
                vm.PersonSelectList.First(x => x.Value == personId.ToString()).Selected = true;

            if (teamId != null && teamId != Guid.Empty)
                vm.TeamSelectList.First(x => x.Value == teamId.ToString()).Selected = true;

            if (roleId != null && roleId != Guid.Empty)
                vm.RoleSelectList.First(x => x.Value == roleId.ToString()).Selected = true;
            
            return vm;
        }
    }
}