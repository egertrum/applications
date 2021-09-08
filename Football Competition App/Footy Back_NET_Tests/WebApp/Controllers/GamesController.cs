using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Base;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using WebApp.ViewModels.Games;
using GameType = Domain.App.GameType;
using Team = Domain.App.Team;

namespace WebApp.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IAppBLL _bll;
        private GameMapper _gameMapper;
        private GameLengthMapper _gameLengthMapper;

        public GamesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            IMapper mapper1 = mapper;
            _gameMapper = new GameMapper(mapper1);
            _gameLengthMapper = new GameLengthMapper(mapper1);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var vm = new GamesIndexViewModel()
            {
                Games = (await _bll.Games.GetAllAsync()).Select(x => _gameMapper.Map(x)!),
                Title = Base.Resources.DTO.v1.Game.AdminGames
            };
            return View("Index", vm);
        }

        public async Task<IActionResult> TeamManagerIndex()
        {
            var vm = new GamesIndexViewModel()
            {
                Games = (await _bll.Games.GetAllUserGames(User.GetUserId()!.Value))
                    .Select(x => _gameMapper.Map(x)!),
                TeamManagerGames = true,
                Title = Base.Resources.DTO.v1.Game.TeamManagerGames
            };
            return View("Index", vm);
        }
        
        public async Task<IActionResult> OrganiserIndex()
        {
            var vm = new GamesIndexViewModel()
            {
                Games = (await _bll.Games.GetAllOrganiserGames(User.GetUserId()!.Value))
                    .Select(x => _gameMapper.Map(x)!),
                OrganiserGames = true,
                Title = Base.Resources.DTO.v1.Game.OrganiserGames
            };
            return View("Index", vm);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var game = _gameMapper.Map(await _bll.Games.FirstOrDefaultAsync(id.Value));
            if (game == null) return NotFound();

            return View(game);
        }
        
        public async Task<IActionResult> Create(Guid? competitionId)
        {
            if (competitionId == null) return RedirectToAction("Index", "Competitions");
            
            var userCompetition =
                await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, competitionId!.Value);
            if (!userCompetition && !User.IsInRole("Admin")) return RedirectToAction("Index", "Competitions");
            
            var teamIds = (await _bll.CompetitionTeams.GetAllTeamIdsByCompetitionId(competitionId)).ToList();
            var vm = new GameCreateEditViewModel
            {
                HomeTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                    nameof(Team.Name)),
                AwayTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                    nameof(Team.Name)),
                GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(), nameof(GameType.Id),
                    nameof(GameType.Name))
            };
            vm.competitionId = competitionId!.Value;
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameCreateEditViewModel vm)
        {
            var userCompetition =
                await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, vm.competitionId);
            if (!userCompetition && !User.IsInRole("Admin")) return RedirectToAction("Index", "Competitions");
            
            if (ModelState.IsValid)
            {
                var gameType = await _bll.GameTypes.FirstOrDefaultAsync(vm.Game.GameTypeId);
                if (!(gameType!.Calling != EGameTypeCalling.Normal && vm.GameLength.ExtraTimeHalfLength == null))
                {
                    vm.Game.CompetitionId = vm.competitionId;
                    var bllGame = _gameMapper.Map(vm.Game);
                    var bllGameLength = _gameLengthMapper.Map(vm.GameLength)!;
                    var addedGame = _bll.Games.Add(bllGame!);
                    await _bll.SaveChangesAsync();
                    await _bll.Games.AddGameParts(addedGame, bllGameLength);
                    return RedirectToAction(nameof(Index));
                }
                vm.ExtraTimeError = Base.Resources.DTO.v1.Game.ExtraTimeError;
            }
            var teamIds = (await _bll.CompetitionTeams.GetAllTeamIdsByCompetitionId(vm.competitionId)).ToList();
            vm.HomeTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                nameof(Team.Name), vm.Game.HomeId);
            vm.AwayTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                nameof(Team.Name), vm.Game.AwayId);
            vm.GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(), nameof(GameType.Id),
                nameof(GameType.Name), vm.Game.GameTypeId);
            
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var game = _gameMapper.Map(await _bll.Games.FirstOrDefaultAsync(id.Value));
            if (game == null) return NotFound();

            var normalTimeLength = await _bll.GameParts.GetNormalTimeLength(id.Value);
            var extraTimeLength = await _bll.GameParts.GetExtraTimeLength(id.Value);
            
            var teamIds = (await _bll.CompetitionTeams.GetAllTeamIdsByCompetitionId(game.CompetitionId)).ToList();
            var vm = new GameCreateEditViewModel
            {
                Game = game,
                GameLength = new GameLength()
                {
                    HalfLength = normalTimeLength,
                    ExtraTimeHalfLength = extraTimeLength
                },
                HomeTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                    nameof(Team.Name), game.HomeId),
                AwayTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                    nameof(Team.Name), game.AwayId),
                GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(), nameof(GameType.Id),
                    nameof(GameType.Name), game.GameTypeId)
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, GameCreateEditViewModel vm)
        {
            if (id != vm.Game.Id) return NotFound();
            
            if (!await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, vm.Game.CompetitionId)
            && !User.IsInRole("Admin"))
                return RedirectToAction("Index", "Competitions");

            var gameType = await _bll.GameTypes.FirstOrDefaultAsync(vm.Game.GameTypeId);
            if (!ModelState.IsValid || gameType!.Calling != EGameTypeCalling.Normal && vm.GameLength.ExtraTimeHalfLength == null)
            {
                var teamIds = (await _bll.CompetitionTeams.GetAllTeamIdsByCompetitionId(vm.Game.CompetitionId)).ToList();
                
                vm.HomeTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                    nameof(Team.Name), vm.Game.HomeId);
                vm.AwayTeamSelectList = new SelectList(await _bll.Teams.GetAllTeamsByIds(teamIds), nameof(Team.Id),
                    nameof(Team.Name), vm.Game.AwayId);
                vm.GameTypeSelectList = new SelectList(await _bll.GameTypes.GetAllAsync(), nameof(GameType.Id),
                    nameof(GameType.Name), vm.Game.GameTypeId);
                
                vm.GameLength.HalfLength = await _bll.GameParts.GetNormalTimeLength(id);
                vm.GameLength.ExtraTimeHalfLength = await _bll.GameParts.GetExtraTimeLength(id);
                return View(vm);
            }

            var bllGame = _gameMapper.Map(vm.Game);
            var bllGameLength = _gameLengthMapper.Map(vm.GameLength)!;
            var updated = await _bll.Games.UpdateGameAndGameParts(bllGame!, bllGameLength);
            if (!updated) return BadRequest();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var game = _gameMapper.Map(await _bll.Games.FirstOrDefaultAsync(id.Value));
            if (game == null) return NotFound();

            return View(game);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)

        {
            var game = await _bll.Games.FirstOrDefaultAsync(id);
            if (game == null) return NotFound();
            
            if (await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, game.CompetitionId) ==
                false)
            {
                return BadRequest();   
            }
            
            await _bll.Games.DeleteGameParts(id);
            await _bll.SaveChangesAsync();
            await _bll.Games.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
