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
using WebApp.ViewModels.Competitions;
using WebApp.ViewModels.Teams;

namespace WebApp.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly IAppBLL _bll;
        
        private TeamMapper _teamMapper;
        private TeamPersonMapper _teamPersonMapper;

        public TeamsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            IMapper mapper1 = mapper;
            _teamMapper = new TeamMapper(mapper1);
            _teamPersonMapper = new TeamPersonMapper(mapper1);
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.Teams.GetAllAsync()).Select(x => _teamMapper.Map(x));
            
            var vm = new TeamIndexViewModel()
            {
                Teams = res!,
                Title = Base.Resources.DTO.v1.Team.Teams,
                UserTeams = false
            };
            
            return View(vm);
        }
        
        public async Task<IActionResult> MyIndex(string? msg, string? personIdCode)
        {
            var res = 
                (await _bll.Teams.GetAllAsync(User.GetUserId()!.Value))
                .Select(x => _teamMapper.Map(x));

            var vm = new TeamIndexViewModel()
            {
                Teams = res!,
                Title = Base.Resources.DTO.v1.Team.MyTeams,
                UserTeams = true,
                Message = msg != null ? Base.Resources.DTO.v1.Person.MemberExists + personIdCode : null
            };
            return View("Index", vm);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var team = _teamMapper.Map(await _bll.Teams.FirstOrDefaultAsync(id.Value));
            if (team == null) return NotFound();

            var vm = new TeamCreateEditDetailsViewModel();
            vm.Team = team;
            vm.BelongsToUser = team.AppUserId == User.GetUserId();
            vm.TeamPersons = (await _bll.TeamPersons.GetAllWithTeamId(team.Id)).Select(x => _teamPersonMapper.Map(x))!;
            
            return View(vm);
        }
        
        public async Task<IActionResult> Create()
        {
            var vm = new TeamCreateEditDetailsViewModel();
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name));
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamCreateEditDetailsViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var addTeam = _teamMapper.Map(vm.Team);
                addTeam!.AppUserId = User.GetUserId()!.Value;
                _bll.Teams.Add(addTeam!);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(MyIndex));
            }
            
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Team.CountryId);
            
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var team = _teamMapper.Map(await _bll.Teams.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value));
            if (team == null) return NotFound();

            var vm = new TeamCreateEditDetailsViewModel();
            vm.Team = team; 
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Team.CountryId);
            
            Console.WriteLine(vm.Team.AppUserId);
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TeamCreateEditDetailsViewModel vm)
        {
            if (id != vm.Team.Id) return NotFound();
            if (vm.Team.AppUserId != User.GetUserId()!.Value) {return RedirectToAction("MyIndex");}
            if (!ModelState.IsValid) return View(vm);

            var bllTeam = _teamMapper.Map(vm.Team);
            
            _bll.Teams.Update(bllTeam!);
            await _bll.SaveChangesAsync();

            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Team.CountryId);
            return RedirectToAction(nameof(MyIndex));
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid? id, string? error)
        {
            if (id == null) return NotFound();
            
            var team = _teamMapper.Map(await _bll.Teams.FirstOrDefaultAsync(id.Value));
            if (team == null) return NotFound();
            
            var vm = new TeamDeleteViewModel() { Team = team };
            
            if (error != null)
                vm.Error = Base.Resources.DTO.v1.Team.Error;

            return View(vm);
        }
        
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var team = await _bll.Teams.FirstOrDefaultAsync(id);
            
            try
            {
                await _bll.Teams.RemoveAsync(id);
                await _bll.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction("Delete", new {id = team!.Id, error = true});
            }
            
            return RedirectToAction(nameof(Index));
        }
        
    }
}
