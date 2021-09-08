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
using WebApp.ViewModels.PollQuestion;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PollQuestionController : Controller
    {
        private readonly AppDbContext _context;

        public PollQuestionController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Create(Guid? questionId, Guid? pollId)
        {
            var vm = new PollQuestionCreateViewModel()
            {
                QuestionsSelectList = new SelectList(_context.Question.Where(q => q.QuestionType == EQuestionType.Poll).ToList(), "Id", "Value"),
                PollsSelectList = new SelectList(_context.Poll.ToList(), "Id", "Name")
            };
            
            if (questionId != null)
                vm.QuestionsSelectList.First(x => x.Value == questionId.ToString()).Selected = true;
            if (pollId != null)
                vm.PollsSelectList.First(x => x.Value == pollId.ToString()).Selected = true;

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PollQuestionCreateViewModel vm)
        {
            vm.PollQuestion.Number = _context.PollQuestion.Count(q => q.PollId == vm.PollQuestion.PollId) + 1;
            
            if (ModelState.IsValid)
            {
                _context.Add(vm.PollQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Poll");
            }
            vm.QuestionsSelectList = new SelectList(_context.Question.Where(q => q.QuestionType == EQuestionType.Poll), "Id", "Value", vm.PollQuestion.QuestionId);
            vm.PollsSelectList = new SelectList(_context.Poll, "Id", "Name", vm.PollQuestion.PollId);
            return View(vm);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pollQuestion = await _context.PollQuestion
                .Include(p => p.Poll)
                .Include(p => p.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pollQuestion == null)
            {
                return NotFound();
            }

            return View(pollQuestion);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pollQuestion = await _context.PollQuestion.FindAsync(id);

            var pollId = pollQuestion.PollId;
            _context.PollQuestion.Remove(pollQuestion);
            await _context.SaveChangesAsync();
            
            var counter = 1;
            var pollQuestions = await _context.PollQuestion.Where(p => p.PollId == pollId).ToListAsync();
            foreach (var question in pollQuestions)
            {
                question.Number = counter;
                _context.PollQuestion.Update(question);
                await _context.SaveChangesAsync();
                counter++;
            }
            
            return RedirectToAction(nameof(Index), "Poll");
        }
    }
}
