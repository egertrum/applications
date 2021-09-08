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
using WebApp.ViewModels.QuizQuestion;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class QuizQuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuizQuestionController(AppDbContext context)
        {
            _context = context;
        }
        
        public IActionResult Create(Guid? questionId, Guid? quizId)
        {
            var vm = new QuizQuestionCreateViewModel()
            {
                QuestionsSelectList = new SelectList(_context.Question.Where(q => q.QuestionType == EQuestionType.Quiz).ToList(), "Id", "Value"),
                QuizzesSelectList = new SelectList(_context.Quiz.ToList(), "Id", "Name")
            };
            
            if (questionId != null)
                vm.QuestionsSelectList.First(x => x.Value == questionId.ToString()).Selected = true;
            if (quizId != null)
                vm.QuizzesSelectList.First(x => x.Value == quizId.ToString()).Selected = true;

            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuizQuestionCreateViewModel vm)
        {
            vm.QuizQuestion.Number = _context.QuizQuestion.Count(q => q.QuizId == vm.QuizQuestion.QuizId) + 1;
            
            var quiz = await _context.Quiz.FindAsync(vm.QuizQuestion.QuizId);
            quiz.MaxPoints = vm.QuizQuestion.Number;
            _context.Quiz.Update(quiz);
            await _context.SaveChangesAsync();
            
            if (ModelState.IsValid)
            {
                _context.Add(vm.QuizQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Quiz");
            }
            vm.QuestionsSelectList = new SelectList(_context.Question.Where(q => q.QuestionType == EQuestionType.Quiz), "Id", "Value", vm.QuizQuestion.QuestionId);
            vm.QuizzesSelectList = new SelectList(_context.Quiz, "Id", "Name", vm.QuizQuestion.QuizId);
            return View(vm);
        }
        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizQuestion = await _context.QuizQuestion
                .Include(q => q.Question)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            return View(quizQuestion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var quizQuestion = await _context.QuizQuestion.FindAsync(id);
            _context.QuizQuestion.Remove(quizQuestion);
            await _context.SaveChangesAsync();
            
            var counter = 1;
            var quizQuestions = await _context.QuizQuestion.ToListAsync();
            foreach (var question in quizQuestions)
            {
                question.Number = counter;
                _context.QuizQuestion.Update(question);
                await _context.SaveChangesAsync();
                counter++;
            }
            
            return RedirectToAction(nameof(Index), "Quiz");
        }
    }
}
