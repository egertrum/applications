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
using BLL.App.DTO;
using BLL.App.DTO.Identity;
using DAL.App.EF;
using Domain.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using PublicApi.DTO.v1.Mappers;
using WebApp.ViewModels.Competitions;
using Competition = BLL.App.DTO.Competition;
using Country = DAL.App.DTO.Country;
using Registration = BLL.App.DTO.Registration;

namespace WebApp.Controllers
{
    [Authorize]
    public class CompetitionsController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.CompetitionMapper _competitionMapper;
        private PublicApi.DTO.v1.Mappers.CompetitionTeamMapper _competitionTeamMapper;
        private PublicApi.DTO.v1.Mappers.GameMapper _gameMapper;
        private readonly IMapper Mapper;

        public CompetitionsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            Mapper = mapper;
            _competitionMapper = new CompetitionMapper(Mapper);
            _competitionTeamMapper = new CompetitionTeamMapper(Mapper);
            _gameMapper = new GameMapper(Mapper);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index(bool? error, string? message, Guid? countryId, string? name, DateTime? startDate, string? reset)
        {
            var vm = new CompetitionIndexViewModel
            {
                Title = "Competitions",
                UserCompetitions = false,
                SearchCompetition = new PublicApi.DTO.v1.SearchCompetition()
                {
                    CountryId = countryId,
                    StartDate = startDate,
                    Name = name
                },
                CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                    nameof(Country.Name)),
            };

            var comp = new PublicApi.DTO.v1.Competition()
            {
                CountryId = countryId ?? Guid.Empty,
                StartDate = startDate ?? DateTime.MinValue,
                Name = name ?? ""
            };

            vm.Competitions = (await _bll.Competitions.GetAllAsyncWithSearch(_competitionMapper.Map(comp)!))
                .Select(x => _competitionMapper.Map(x)!);
            if (error != null) vm.Error = Base.Resources.Views.Shared.Basic.NotAllowed;
            if (message != null) vm.Message = message;
            return View(vm);
        }
        
        public async Task<IActionResult> MyIndex()
        {
            var res = 
                (await _bll.Competitions.GetAllAsync(User.GetUserId()!.Value))
                .Select(x => _competitionMapper.Map(x));
            
            var vm = new CompetitionIndexViewModel
            {
                Competitions = res!,
                Title = Base.Resources.DTO.v1.Competition.MyCompetitions,
                UserCompetitions = true
            };
            return View("Index", vm);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var competition = _competitionMapper.Map(await _bll.Competitions.FirstOrDefaultAsync(id.Value));
            
            if (competition == null) return NotFound();
            
            var competitionTeams = (await _bll.CompetitionTeams.GetAllByCompetitonId(competition.Id))
                .Select(x => _competitionTeamMapper.Map(x));
            
            var games = (await _bll.Games.GetAllByCompetitionId(competition.Id))
                .Select(x => _gameMapper.Map(x));

            var vm = new CompetitionDetailsViewModel()
            {
                Competition = competition,
                CompetitionTeams = competitionTeams!,
                Games = games!
            };
            return View(vm);
        }

        
        public async Task<IActionResult>  Create()
        {
            var vm = new CompetitionCreateEditViewModel
            {
                CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                    nameof(Country.Name))
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetitionCreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var bllComp = _competitionMapper.Map(vm.Competition);
                var addedComp = _bll.Competitions.Add(bllComp!);

                await _bll.Competitions.MakeRegistration(User.GetUserId()!.Value, addedComp.Id);
                return RedirectToAction(nameof(Index));
            }
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Competition.CountryId);
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            if (!User.IsInRole("Admin") && !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id.Value))
                return RedirectToAction("Index",  new { error = true });

            var competition = _competitionMapper.Map(await _bll.Competitions.FirstOrDefaultAsync(id.Value));
            if (competition == null) return NotFound();

            var vm = new CompetitionCreateEditViewModel {Competition = competition};
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Competition.CountryId);
            
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CompetitionCreateEditViewModel vm)
        {
            if (id != vm.Competition.Id) return NotFound();
            
            if (!User.IsInRole("Admin") && !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id))
                return RedirectToAction("Index",  new { error = true });

            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Competition.CountryId);
            
            if (!ModelState.IsValid) return View(vm);

            var bllComp = _competitionMapper.Map(vm.Competition);
            _bll.Competitions.Update(bllComp!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            if (!User.IsInRole("Admin") && !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id.Value))
                return RedirectToAction(nameof(Index),  new { error = true });

            var competition = _competitionMapper.Map(await _bll.Competitions.FirstOrDefaultAsync(id.Value));
            if (competition == null) return NotFound();

            return View(competition);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!User.IsInRole("Admin") && !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id))
                return RedirectToAction("Index",  new { error = true });
            
            var reg = await _bll.Registrations.GetByCompetitionId(id);

            try
            {
                _bll.Registrations.Remove(reg);
                await _bll.Competitions.RemoveAsync(id);
                await _bll.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new {error = true});
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
