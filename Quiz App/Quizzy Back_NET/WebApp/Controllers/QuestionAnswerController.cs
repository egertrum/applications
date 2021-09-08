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
using WebApp.ViewModels.QuestionAnswer;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuestionAnswerController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionAnswerController(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(Guid? id)
        {
            var appDbContext = _context.QuestionAnswer
                .Include(q => q.Question)
                .OrderBy(q => q.Question!.Value);
            var vm = new QuestionAnswerIndexViewModel()
            {
                QuestionAnswers = id != null
                    ? await appDbContext.Where(a => a.QuestionId == id).ToListAsync()
                    : await appDbContext.ToListAsync(),
                QuestionId = id,
                Title = id != null ? "Answers to question" : "Answers to all questions"
            };
            return View(vm);
        }
        
        public async Task<IActionResult> Create(Guid? questionId)
        {
            var appDbContext = _context.Question;
            var vm = new QuestionAnswerCreateViewModel();
            
            if (questionId != null)
            {
                var question = await _context.Question.FindAsync(questionId);
                if (question.QuestionType == EQuestionType.Poll)
                {
                    vm.QuestionsSelectList = new SelectList(appDbContext
                        .Where(q => q.QuestionType == EQuestionType.Poll)
                        .ToList(), "Id", "Value");
                    vm.PollQuestion = true;
                }
                else
                {
                    vm.QuestionsSelectList = new SelectList(appDbContext
                        .Where(q => q.QuestionType == EQuestionType.Quiz)
                        .ToList(), "Id", "Value");
                }
            }
            else
            {
                vm.QuestionsSelectList = new SelectList(appDbContext.ToList(), "Id", "Value");   
            }

            if (questionId != null)
            {
                vm.QuestionsSelectList!.First(x => x.Value == questionId.ToString()).Selected = true;    
            }
            

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuestionAnswerCreateViewModel vm, string? add)
        {
            var question = await _context.Question.FindAsync(vm.QuestionAnswer.QuestionId);
            if (question.QuestionType == EQuestionType.Poll)
            {
                vm.QuestionAnswer.True = true;
            }
            
            if (ModelState.IsValid)
            {
                _context.Add(vm.QuestionAnswer);
                await _context.SaveChangesAsync();
                return add != null ? RedirectToAction(nameof(Create),  new { questionId = vm.QuestionAnswer!.QuestionId }) 
                    : RedirectToAction(nameof(Index));

            }
            if (question.QuestionType == EQuestionType.Poll)
            {
                vm.QuestionsSelectList = new SelectList(_context.Question
                    .Where(q => q.QuestionType == EQuestionType.Poll), "Id", "Value", vm.QuestionAnswer.QuestionId);
            }
            else
            {
                vm.QuestionsSelectList = new SelectList(_context.Question
                    .Where(q => q.QuestionType == EQuestionType.Quiz), "Id", "Value", vm.QuestionAnswer.QuestionId);   
            }
            return View(vm);
        }
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswer.FindAsync(id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            var vm = new QuestionAnswerCreateViewModel()
            {
                QuestionAnswer = questionAnswer,
                QuestionsSelectList = new SelectList(_context.Question, "Id", "Value", questionAnswer.QuestionId)
            };
            
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuestionAnswerCreateViewModel vm)
        {
            if (id != vm.QuestionAnswer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.QuestionAnswer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionAnswerExists(vm.QuestionAnswer.Id))
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
            vm.QuestionsSelectList = new SelectList(_context.Question, "Id", "Value", vm.QuestionAnswer.QuestionId);
            return View(vm);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var questionAnswer = await _context.QuestionAnswer
                .Include(q => q.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (questionAnswer == null)
            {
                return NotFound();
            }

            return View(questionAnswer);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var questionAnswer = await _context.QuestionAnswer.FindAsync(id);
            var userAnswers = await _context.UserAnswer.Where(u => u.QuestionAnswerId == questionAnswer.Id).ToListAsync();
                foreach (var answer in userAnswers)
                {
                    _context.UserAnswer.Remove(answer);
                    await _context.SaveChangesAsync();
                }
            _context.QuestionAnswer.Remove(questionAnswer);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool QuestionAnswerExists(Guid id)
        {
            return _context.QuestionAnswer.Any(e => e.Id == id);
        }
    }
}
