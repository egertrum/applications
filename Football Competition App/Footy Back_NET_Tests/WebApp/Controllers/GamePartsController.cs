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
using PublicApi.DTO.v1.Mappers;
using WebApp.ViewModels.GameParts;

namespace WebApp.Controllers
{
    public class GamePartsController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.GamePartMapper _gamePartMapper;

        public GamePartsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _gamePartMapper = new GamePartMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.GameParts.GetAllAsync()).Select(x => _gamePartMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var gamePart = _gamePartMapper.Map(await _bll.GameParts.FirstOrDefaultAsync(id.Value));
            if (gamePart == null) return NotFound();

            return View(gamePart);
        }
        
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditViewModel()
            {
                GameSelectList = new SelectList(await _bll.Games.GetAllAsync(), "Id", "Name"),
                GamePartTypeSelectList = new SelectList(await _bll.GamePartTypes.GetAllAsync(), "Id", "Name")
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublicApi.DTO.v1.GamePart gamePart, CreateEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _bll.GameParts.Add(_gamePartMapper.Map(gamePart)!);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.GamePart = gamePart;
            vm.GameSelectList = new SelectList(await _bll.Games.GetAllAsync(), "Id", "Name", gamePart.GameId);
            vm.GamePartTypeSelectList = new SelectList(await _bll.GamePartTypes.GetAllAsync(), "Id", "Name", gamePart.GamePartTypeId);
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var gamePart = _gamePartMapper.Map(await _bll.GameParts.FirstOrDefaultAsync(id.Value));
            if (gamePart == null) return NotFound();
            var vm = new CreateEditViewModel();
            vm.GamePart = gamePart;
            vm.GameSelectList = new SelectList(await _bll.Games.GetAllAsync(), "Id", "Name", gamePart.GameId);
            vm.GamePartTypeSelectList = new SelectList(await _bll.GamePartTypes.GetAllAsync(), "Id", "Name", gamePart.GamePartTypeId); 
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditViewModel vm)
        {
            if (id != vm.GamePart.Id) return NotFound();
            
            vm.GameSelectList = new SelectList(await _bll.Games.GetAllAsync(), "Id", "Name", vm.GamePart.GameId);
            vm.GamePartTypeSelectList = new SelectList(await _bll.GamePartTypes.GetAllAsync(), "Id", "Name", vm.GamePart.GamePartTypeId); 
            
            if (!ModelState.IsValid) return View(vm);

            var bllGamePart = _gamePartMapper.Map(vm.GamePart);
            _bll.GameParts.Update(bllGamePart!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var gamePart = _gamePartMapper.Map(await _bll.GameParts.FirstOrDefaultAsync(id.Value));
            if (gamePart == null) return NotFound();

            return View(gamePart);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.GameParts.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
