using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.Identity;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using DAL.App.EF.Mappers;
using Domain.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using AppUserMapper = PublicApi.DTO.v1.Mappers.AppUserMapper;
using Report = PublicApi.DTO.v1.Report;
using ReportMapper = PublicApi.DTO.v1.Mappers.ReportMapper;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<Domain.App.Identity.AppUser> _userManager;
        private ReportMapper _reportMapper;

        public ReportsController(IAppBLL bll, UserManager<Domain.App.Identity.AppUser> userManager, IMapper mapper)
        {
            _bll = bll;
            _userManager = userManager;
            _reportMapper = new ReportMapper(mapper);
        }
        
        public async Task<IActionResult> Index()
        {
            var res = (await _bll.Reports.GetAllAsync()).Select(x => _reportMapper.Map(x));
            return View(res);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var report = _reportMapper.Map(await _bll.Reports.FirstOrDefaultAsync(id.Value));
            if (report == null) return NotFound();

            return View(report);
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Report report)
        {
            var bllReport = _reportMapper.Map(report);
            
            if (!ModelState.IsValid) return View(report);
            var user = await _userManager.GetUserAsync(User);
            var bllAndSubmitter = _bll.Reports.AddSubmitter(bllReport!, user);
            _bll.Reports.Add(bllAndSubmitter!);
            await _bll.SaveChangesAsync();
            
            var msg = Base.Resources.DTO.v1.Report.Feedback;
            return RedirectToAction("Index", "Competitions", new { message = msg });
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var report = _reportMapper.Map(await _bll.Reports.FirstOrDefaultAsync(id.Value));
            if (report == null) return NotFound();

            return View(report);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Reports.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
