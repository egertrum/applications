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
    public class GamePartTypesController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.GamePartTypeMapper _gamePartTypeMapper;

        public GamePartTypesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _gamePartTypeMapper = new GamePartTypeMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.GamePartTypes.GetAllAsync()).Select(x => _gamePartTypeMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var gamePartType = _gamePartTypeMapper.Map(await _bll.GamePartTypes.FirstOrDefaultAsync(id.Value));
            if (gamePartType == null) return NotFound();

            return View(gamePartType);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublicApi.DTO.v1.GamePartType gamePartType)
        {
            if (!ModelState.IsValid) return View(gamePartType);
            _bll.GamePartTypes.Add(_gamePartTypeMapper.Map(gamePartType)!);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var gamePartType = _gamePartTypeMapper.Map(await _bll.GamePartTypes.FirstOrDefaultAsync(id.Value));
            if (gamePartType == null) return NotFound();
            return View(gamePartType);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PublicApi.DTO.v1.GamePartType gamePartType)
        {
            if (id != gamePartType.Id) return NotFound();

            if (!ModelState.IsValid) return View(gamePartType);

            var bllGamePartType = _gamePartTypeMapper.Map(gamePartType);
            _bll.GamePartTypes.Update(bllGamePartType!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var gamePartType = _gamePartTypeMapper.Map(await _bll.GamePartTypes.FirstOrDefaultAsync(id.Value));
            if (gamePartType == null) return NotFound();

            return View(gamePartType);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.GamePartTypes.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
