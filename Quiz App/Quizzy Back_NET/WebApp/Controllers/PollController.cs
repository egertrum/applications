using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels.Poll;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PollController : Controller
    {
        private readonly AppDbContext _context;

        public PollController(AppDbContext context)
        {
            _context = context;
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> Index(bool? error, bool? done)
        {
            var vm = new PollIndexViewModel()
            {
                Polls = await _context.Poll.ToListAsync()
            };
            if (error != null)
            {
                vm.Error = "Can not delete unless you try to delete all of the sequences of this poll questions.";
            }

            if (done != null)
            {
                vm.Done = "Thank you for your feedback!";
            }
            return View(vm);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            

            var poll = await _context.Poll
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null) return NotFound();
            
            var vm = new PollDetailsViewModel()
            {
                Poll = poll,
                PollQuestions = await _context.PollQuestion
                    .Include(q => q.Question)
                    .Where(q => q.PollId == poll.Id)
                    .OrderBy(q => q.Number)
                    .ToListAsync()
            };

            return View(vm);
        }
        
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Poll poll)
        {
            if (ModelState.IsValid)
            { 
                _context.Add(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Poll.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }
            return View(poll);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id")] Poll poll)
        {
            if (id != poll.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poll);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PollExists(poll.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(poll);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var poll = await _context.Poll
                .FirstOrDefaultAsync(m => m.Id == id);
            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var poll = await _context.Poll.FindAsync(id);
            try
            {
                _context.Poll.Remove(poll);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { error = true });
            }
        }

        private bool PollExists(Guid id)
        {
            return _context.Poll.Any(e => e.Id == id);
        }
    }
}
