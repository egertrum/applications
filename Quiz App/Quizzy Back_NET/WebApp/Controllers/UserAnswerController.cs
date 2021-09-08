using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authorization;
using WebApp.ViewModels.UserAnswer;

namespace WebApp.Controllers
{
    public class UserAnswerController : Controller
    {
        private readonly AppDbContext _context;

        public UserAnswerController(AppDbContext context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserAnswer.Include(u => u.AppUser).Include(u => u.QuestionAnswer);
            return View(await appDbContext.ToListAsync());
        }
        
        public async Task<IActionResult> FeedBack(Guid? uniqueId, Guid? quizId)
        {
            var questionAndAnswers = await _context.UserAnswer
                .Include(u => u.AppUser)
                .Include(u => u.QuestionAnswer)
                .ThenInclude(q => q!.Question)
                .Where(q => q.UniqueQuizId == uniqueId)
                .ToListAsync();

            var rightCount = _context.UserAnswer
                .Include(u => u.QuestionAnswer)
                .ThenInclude(q => q!.Question)
                .Where(q => q.UniqueQuizId == uniqueId)
                .Count(q => q.QuestionAnswer!.True);

            var max = _context.UserAnswer
                .Include(u => u.QuestionAnswer)
                .ThenInclude(q => q!.Question)
                .Count(q => q.UniqueQuizId == uniqueId);

            if (quizId != null)
            {
                var score = new Score()
                {
                    QuizId = quizId.Value,
                    Amount = rightCount,
                    Passed = (rightCount / (double) max * 100) > 50
                };
                await _context.Score.AddAsync(score);
                await _context.SaveChangesAsync();
                
                var quiz = await _context.Quiz.FindAsync(quizId);
                var allScores = _context.Score!.Count(r => r.QuizId == quizId);
                quiz.PassingProc = (int)(_context.Score.Count(r => r.Passed && r.QuizId == quizId) / Convert.ToDouble(allScores) * 100);
                
                var resultsSum = 0;
                var scores = await _context.Score.Where(r => r.QuizId == quizId).ToListAsync();
                foreach (var res in scores)
                {
                    resultsSum += res.Amount;
                }
                quiz.Average = Math.Round(resultsSum / Convert.ToDouble(_context.Score!.Count(r => r.QuizId == quizId)), 2);
                
                _context.Quiz.Update(quiz);
                await _context.SaveChangesAsync();
            }
            
            var vm = new UserAnswerFeedBackViewModel()
            {
                UserAnswers = questionAndAnswers,
                MaxPoints = max,
                CorrectAnswers = rightCount
            };
                
            return View(vm);
        }
        
        public IActionResult Create(Guid id, int number, Guid? uniqueId, bool? poll)
        {

            var question = poll != null
                ? (_context.PollQuestion.Include(q => q.Question)
                    .First(q => q.PollId == id && q.Number == number)).Question!
                : (_context.QuizQuestion.Include(q => q.Question)
                    .First(q => q.QuizId == id && q.Number == number)).Question!;

            var answers = _context.QuestionAnswer
                .Where(q => q.QuestionId == question!.Id)
                .OrderBy(q => Guid.NewGuid())
                .ToList();

            var questionsCount = poll != null 
                ? _context.PollQuestion.Count(q => q.PollId == id) 
                : _context.QuizQuestion.Count(q => q.QuizId == id);
            
            var vm = new UserAnswerCreateViewModel()
            {
                QuizId = id,
                Question = question!.Value,
                QuestionAnswersSelectList = new SelectList(answers, "Id", "Value"),
                NextNumber = number + 1,
                Poll = poll
            };
            vm.LastQuestion = questionsCount == number;
            vm.QuizUniqueId = uniqueId ?? Guid.NewGuid();
            return View(vm);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAnswerCreateViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);
            
            if (User.Identity?.IsAuthenticated ?? false)
            {
                vm.UserAnswer.AppUserId = User.GetUserId()!.Value;   
            }
            
            var question = _context.QuestionAnswer
                .Include(q => q.Question)
                .FirstOrDefault(q => q.Id == vm.UserAnswer.QuestionAnswerId)!
                .Question;
            
            var exists = _context.UserAnswer
                .FirstOrDefault(u => u.QuestionAnswer!.QuestionId == question!.Id && u.UniqueQuizId == vm.QuizUniqueId);
            if (exists != null)
            {
                return vm.Poll == null 
                    ? RedirectToAction(nameof(FeedBack), new {uniqueId = vm.QuizUniqueId}) 
                    : RedirectToAction(nameof(Index), "Poll", new {done = true});
            }

            if (vm.Poll == null)
            {
                vm.UserAnswer.QuizId = vm.QuizId;
            }
            vm.UserAnswer.UniqueQuizId = vm.QuizUniqueId;
            _context.Add(vm.UserAnswer);
            await _context.SaveChangesAsync();
            
            var questionsCount = vm.Poll != null 
                ? _context.PollQuestion.Count(q => q.PollId == vm.QuizId) 
                : _context.QuizQuestion.Count(q => q.QuizId == vm.QuizId);

            if (vm.Poll == null)
            {
                return questionsCount < vm.NextNumber 
                    ? RedirectToAction(nameof(FeedBack), new { uniqueId = vm.QuizUniqueId, quizId = vm.QuizId }) 
                    : RedirectToAction(nameof(Create), new { id = vm.QuizId, number = vm.NextNumber, uniqueId = vm.QuizUniqueId });   
            }
            return questionsCount < vm.NextNumber 
                ? RedirectToAction(nameof(Index), "Poll", new {done = true})
                : RedirectToAction(nameof(Create), new { id = vm.QuizId, number = vm.NextNumber, uniqueId = vm.QuizUniqueId, poll = true });
        }
    }
}
