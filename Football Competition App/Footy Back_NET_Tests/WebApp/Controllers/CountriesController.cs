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
    public class CountriesController : Controller
    {
        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.CountryMapper _countryMapper;

        public CountriesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _countryMapper = new CountryMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.Countries.GetAllAsync()).Select(x => _countryMapper.Map(x));
            return View(res);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublicApi.DTO.v1.Country country)
        {
            if (!ModelState.IsValid) return View(country);
            _bll.Countries.Add(_countryMapper.Map(country)!);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var country = _countryMapper.Map(await _bll.Countries.FirstOrDefaultAsync(id.Value));
            if (country == null) return NotFound();
            
            return View(country);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PublicApi.DTO.v1.Country country)
        {
            if (id != country.Id) return NotFound();

            if (!ModelState.IsValid) return View(country);

            var bllCountry = _countryMapper.Map(country);
            _bll.Countries.Update(bllCountry!);
            await _bll.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var country = _countryMapper.Map(await _bll.Countries.FirstOrDefaultAsync(id.Value));
            if (country == null) return NotFound();

            return View(country);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Countries.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
