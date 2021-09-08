using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using DTO.App.V1;
using DTO.App.V1.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using UserAnswer = Domain.App.UserAnswer;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserAnswerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserAnswerMapper _userAnswerMapper;

        public UserAnswerController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _userAnswerMapper = new UserAnswerMapper(mapper);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserAnswer>>> GetUserAnswers()
        {
            return Ok((await _context.UserAnswer.ToListAsync()).Select(x => _userAnswerMapper.Map(x)));
        }
        
        [HttpGet("/api/v{version:apiVersion}/UserAnswer/Feedback")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserAnswerFeedback>>> GetUserFeedBack(Guid? uniqueId, Guid? quizId)
        {
            var questionAndAnswers = await _context.UserAnswer
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
                var score = new Domain.App.Score()
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
            
            var feedBack = new UserAnswerFeedback()
            {
                UserAnswers = questionAndAnswers.Select(x => _userAnswerMapper.Map(x)!),
                MaxPoints = max,
                CorrectAnswers = rightCount
            };
            
            return Ok(feedBack);
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.UserAnswer>> GetUserAnswer(Guid id)
        {
            var userAnswer = _userAnswerMapper.Map(await _context.UserAnswer.FindAsync(id));

            if (userAnswer == null)
            {
                return NotFound();
            }

            return userAnswer;
        }
        
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<DTO.App.V1.UserAnswer>> PostUserAnswer(DTO.App.V1.UserAnswer userAnswer)
        {
            userAnswer.UniqueQuizId ??= Guid.NewGuid(); //if null then new guid
            var added = await _context.UserAnswer.AddAsync(_userAnswerMapper.Map(userAnswer)!);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAnswer", new { id = userAnswer.UniqueQuizId }, added.Entity);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserAnswer(Guid id)
        {
            var userAnswer = await _context.UserAnswer.FindAsync(id);
            if (userAnswer == null)
            {
                return NotFound();
            }

            _context.UserAnswer.Remove(userAnswer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
