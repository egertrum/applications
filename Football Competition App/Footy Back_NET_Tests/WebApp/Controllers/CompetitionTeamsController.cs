using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1.Mappers;
using WebApp.ViewModels.CompetitionTeams;

namespace WebApp.Controllers
{
    [Authorize]
    public class CompetitionTeamsController : Controller
    {
        private readonly IAppBLL _bll;
        private CompetitionTeamMapper _competitionTeamMapper;
        private CompetitionMapper _competitionMapper;
        
        public CompetitionTeamsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            IMapper mapper1 = mapper;
            _competitionTeamMapper = new CompetitionTeamMapper(mapper1);
            _competitionMapper = new CompetitionMapper(mapper1);
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.CompetitionTeams.GetAllAsync())
                .Select(x => _competitionTeamMapper.Map(x));

            var vm = new CompetitionTeamIndexModel
            {
                CompetitionTeams = res!,
                Message = Base.Resources.DTO.v1.CompetitionTeam.AllTeamsAtCompetitions,
                UserRegisters = false
            };
            return View(vm);
        }

        public async Task<IActionResult> TeamManagerIndex()
        {
            var res =
                (await _bll.CompetitionTeams.GetAllAsync(User.GetUserId()!.Value))
                .Select(x => _competitionTeamMapper.Map(x));

            var vm = new CompetitionTeamIndexModel
            {
                CompetitionTeams = res!,
                Message = Base.Resources.DTO.v1.CompetitionTeam.MyTeamsAtCompetitions,
                UserRegisters = true
            };
            return View("Index", vm);
        }

        public async Task<IActionResult> OrganiserIndex()
        {
            var res =
                (await _bll.CompetitionTeams.GetAllOrganiserCompetitions(User.GetUserId()!.Value))
                .Select(x => _competitionTeamMapper.Map(x));

            var vm = new CompetitionTeamIndexModel
            {
                CompetitionTeams = res!,
                Message = Base.Resources.DTO.v1.CompetitionTeam.TeamsAttendingMyCompetitions,
                UserRegisters = true
            };
            return View("Index", vm);
        }
        
        public async Task<IActionResult> Create(Guid? compId, Guid? countryId)
        {
            var vm = new CompetitionTeamCreateEditViewModel();
            vm.countryId = countryId;
            vm = await BindSelectListsToViewModel(vm, compId, null);
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetitionTeamCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (!await _bll.CompetitionTeams.TeamExistsAtCompetition(vm.CompetitionTeam.TeamId,
                    vm.CompetitionTeam.CompetitionId))
                {
                    if (!await _bll.Teams.BelongsToUserId(User.GetUserId()!.Value, vm.CompetitionTeam.TeamId))
                        return RedirectToAction(nameof(Index), new {Controller = "Competitions"});    
                    
                    _bll.CompetitionTeams.Add(_competitionTeamMapper.Map(vm.CompetitionTeam)!);
                    await _bll.SaveChangesAsync();
                    return RedirectToAction(nameof(TeamManagerIndex));
                }
                
                vm.Error = Base.Resources.DTO.v1.CompetitionTeam.AlreadyRegistered;
            }

            vm = await BindSelectListsToViewModel(vm, vm.CompetitionTeam.CompetitionId, vm.CompetitionTeam.TeamId);
            return View(vm);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var competitionTeam =
                _competitionTeamMapper.Map(await _bll.CompetitionTeams.FirstOrDefaultAsyncWithEntities(id.Value));
            
            if (competitionTeam == null) return NotFound();

            return View(competitionTeam);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id, Guid competitionId)
        {
            if (!await _bll.CompetitionTeams.BelongsToUserId(User.GetUserId()!.Value, id) &&
                !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, competitionId))
            {
                return RedirectToAction("TeamManagerIndex", new {error = "Not allowed!"});
            }

            await _bll.CompetitionTeams.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {Controller = "Competitions"});
        }

        private async Task<CompetitionTeamCreateEditViewModel> BindSelectListsToViewModel(
            CompetitionTeamCreateEditViewModel vm, Guid? competitionId,
            Guid? teamId)
        {
            var competition = new PublicApi.DTO.v1.Competition()
            {
                CountryId = vm.countryId ?? Guid.Empty
            };
            
            vm.CompetitionSelectList = new SelectList(await _bll.Competitions.GetAllAsyncWithSearch(_competitionMapper.Map(competition)!), 
                nameof(Competition.Id),
                nameof(Competition.Name));
            vm.TeamSelectList = new SelectList(await _bll.Teams.GetAllAsync(User.GetUserId()!.Value), nameof(Team.Id),
                nameof(Team.Name));
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name));

            if (competitionId != null && competitionId != Guid.Empty)
                vm.CompetitionSelectList.First(x => x.Value == competitionId.ToString()).Selected = true;
            
            if (teamId != null && teamId != Guid.Empty)
                vm.TeamSelectList.First(x => x.Value == teamId.ToString()).Selected = true;

            return vm;
        }
    }
}