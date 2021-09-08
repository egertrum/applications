using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App;
using Domain.Base;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels.Question;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(bool? error)
        {
            var vm = new QuestionIndexViewModel()
            {
                Questions = await _context.Question.ToListAsync()
            };
            if (error != null)
            {
                vm.Error =
                    "Can not delete unless you try to delete all of the sequences of this question at Quizzes, Polls and Answers.";
            } 
            return View(vm);
        }
        
        public IActionResult Create()
        {
            var types = new List<EQuestionType> {EQuestionType.Poll, EQuestionType.Quiz};
            var vm = new QuestionCreateViewModel()
            {
                QuestionTypes = types
            };
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.Question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.QuestionTypes = new List<EQuestionType> { EQuestionType.Poll, EQuestionType.Quiz };
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var question = await _context.Question.FindAsync(id);
            if (question == null) return NotFound();
            
            return View(question);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
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
            return View(question);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var question = await _context.Question.FindAsync(id);
            try
            {
                _context.Question.Remove(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index), new { error = true });
            }
        }

        private bool QuestionExists(Guid id)
        {
            return _context.Question.Any(e => e.Id == id);
        }
    }
}
