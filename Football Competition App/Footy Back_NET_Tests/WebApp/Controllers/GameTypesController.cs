using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
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
    public class GameTypesController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.GameTypeMapper _gameTypeMapper;

        public GameTypesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _gameTypeMapper = new GameTypeMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.GameTypes.GetAllAsync()).Select(x => _gameTypeMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var gameType = _gameTypeMapper.Map(await _bll.GameTypes.FirstOrDefaultAsync(id.Value));
            if (gameType == null) return NotFound();

            return View(gameType);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublicApi.DTO.v1.GameType gameType)
        {
            if (!ModelState.IsValid) return View(gameType);
            _bll.GameTypes.Add(_gameTypeMapper.Map(gameType)!);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var gameType = _gameTypeMapper.Map(await _bll.GameTypes.FirstOrDefaultAsync(id.Value));
            if (gameType == null) return NotFound();
            return View(gameType);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PublicApi.DTO.v1.GameType gameType)
        {
            if (id != gameType.Id) return NotFound();

            if (!ModelState.IsValid) return View(gameType);

            var bllGameType = _gameTypeMapper.Map(gameType);
            _bll.GameTypes.Update(bllGameType!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var gameType = _gameTypeMapper.Map(await _bll.GameTypes.FirstOrDefaultAsync(id.Value));
            if (gameType == null) return NotFound();

            return View(gameType);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.GameTypes.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
