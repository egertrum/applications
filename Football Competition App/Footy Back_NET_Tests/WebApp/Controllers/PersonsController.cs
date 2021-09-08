using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.App;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App.Identity;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PublicApi.DTO.v1.Mappers;
using WebApp.ViewModels.Persons;
using Country = Domain.App.Country;
using Team = Domain.App.Team;


namespace WebApp.Controllers
{
    [Authorize]
    public class PersonsController : Controller
    {

        private readonly IAppBLL _bll;
        private PublicApi.DTO.v1.Mappers.PersonMapper _personMapper;

        public PersonsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _personMapper = new PersonMapper(mapper);
        }
        
        public async Task<IActionResult> Index(string? canDelete)
        {
            var vm = new PersonIndexViewModel()
            {
                Persons = User.IsInRole("Admin")
                    ? (await _bll.Persons.GetAllAsync()).Select(x => _personMapper.Map(x)!)
                    : (await _bll.Persons.GetAllAsync(User.GetUserId()!.Value)).Select(x =>
                        _personMapper.Map(x)!),
                errorMessage = canDelete != null ? Base.Resources.DTO.v1.Person.CanNotDelete : null
            };
            return View(vm);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var person = _personMapper.Map(await _bll.Persons.FirstOrDefaultAsyncWithTeams(id.Value));
            if (person == null) return NotFound();

            return View(person);
        }
        
        public async Task<IActionResult> Create()
        {
            var vm = new PersonCreateEditViewModel();
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name));
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonCreateEditViewModel vm, string? add)
        {
            var bllPerson = _personMapper.Map(vm.Person);
            
            if (ModelState.IsValid)
            {
                var alreadyEntered =
                    await _bll.Persons.ExistsByIdentificationCode(vm.Person.IdentificationCode, User.GetUserId()!.Value);
                if (alreadyEntered)
                    return RedirectToAction("MyIndex", "Teams", new { msg = true, personIdCode = vm.Person.IdentificationCode});

                bllPerson!.AppUserId = User.GetUserId()!.Value;
                var addedPerson = _bll.Persons.Add(bllPerson!);
                await _bll.SaveChangesAsync();

                return add != null ? RedirectToAction(nameof(Create), "TeamPersons", new {personId = addedPerson.Id}) 
                    : RedirectToAction(nameof(Index));
            }
            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Person.CountryId);

            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var person = _personMapper.Map(await _bll.Persons.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value));
            if (person == null) return NotFound();

            var vm = new PersonCreateEditViewModel()
            {
                Person = person,
                CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                    nameof(Country.Name), person.CountryId)
            };

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PersonCreateEditViewModel vm)
        {
            if (id != vm.Person.Id) return NotFound();

            if (!ModelState.IsValid || !await _bll.Persons.ExistsAsync(vm.Person.Id, User.GetUserId()!.Value))
                return View(vm);

            var bllPerson = _personMapper.Map(vm.Person);
            bllPerson!.AppUserId = User.GetUserId()!.Value;
            
            _bll.Persons.Update(bllPerson);
            await _bll.SaveChangesAsync();

            vm.CountrySelectList = new SelectList(await _bll.Countries.GetAllAsync(), nameof(Country.Id),
                nameof(Country.Name), vm.Person.CountryId);
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var person = _personMapper.Map(await _bll.Persons.FirstOrDefaultAsync(id.Value, User.GetUserId()!.Value));
            if (person == null) return NotFound();

            return View(person);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            string? del = null;
            try
            {
                await _bll.Persons.RemoveAsync(id, User.GetUserId()!.Value);
                await _bll.SaveChangesAsync();
            }
            catch (Exception)
            {
                del = "1";
                // throws error message at Index page, that can not delete member!
            }

            return RedirectToAction(nameof(Index), new { canDelete = del });
        }
    }
}
