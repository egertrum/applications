using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain.App;
using DTO.App.V1.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuizQuestionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly QuizQuestionMapper _quizQuestionMapper;
        private readonly QuestionMapper _questionMapper;

        public QuizQuestionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _quizQuestionMapper = new QuizQuestionMapper(mapper);
            _questionMapper = new QuestionMapper(mapper);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.QuizQuestion>>> GetQuizQuestions()
        {
            return Ok((await _context.QuizQuestion
                .Include(q => q.Question)
                .Include(q => q.Quiz)
                .OrderBy(q => q.Number)
                .ToListAsync()).Select(x => _quizQuestionMapper.Map(x)));
        }
        
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/QuizQuestion/Max")]
        public ActionResult<int> GetQuizQuestionMaxNumber(Guid quizId)
        {
            return _context.QuizQuestion.Count(q => q.QuizId == quizId);
        }
        
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/QuizQuestion/Quiz")]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.QuizQuestion>>> GetQuizQuestionsByQuizId(Guid quizId)
        {
            return Ok((await _context.QuizQuestion
                .Include(q => q.Question)
                .Where(q => q.QuizId == quizId)
                .OrderBy(q => q.Number)
                .ToListAsync()).Select(x => _quizQuestionMapper.Map(x)));
        }
        
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/QuizQuestion/Quiz/Number")]
        public ActionResult<DTO.App.V1.Question> GetQuizQuestionByQuizIdAndNumber(Guid quizId, int number)
        {
            var question = (_context.QuizQuestion.Include(q => q.Question)
                .First(q => q.QuizId == quizId && q.Number == number)).Question!;
            return Ok(_questionMapper.Map(question));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.QuizQuestion>> GetQuizQuestion(Guid id)
        {
            var quizQuestion = _quizQuestionMapper.Map(await _context.QuizQuestion
                .Include(q => q.Question)
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(q => q.Id == id));

            if (quizQuestion == null)
            {
                return NotFound();
            }

            return quizQuestion;
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.QuizQuestion>> PostQuizQuestion(DTO.App.V1.QuizQuestion quizQuestion)
        {
            quizQuestion.Number = _context.QuizQuestion.Count(q => q.QuizId == quizQuestion.QuizId) + 1;
            
            var quiz = await _context.Quiz.FindAsync(quizQuestion.QuizId);
            quiz.MaxPoints = quizQuestion.Number.Value;
            _context.Quiz.Update(quiz);
            await _context.SaveChangesAsync();
            
            await _context.QuizQuestion.AddAsync(_quizQuestionMapper.Map(quizQuestion)!);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetQuizQuestion", new { id = quizQuestion.Id }, quizQuestion);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuizQuestion(Guid id)
        {
            var quizQuestion = await _context.QuizQuestion.FindAsync(id);
            if (quizQuestion == null)
            {
                return NotFound();
            }

            var quizId = quizQuestion.QuizId;
            
            _context.QuizQuestion.Remove(quizQuestion);
            await _context.SaveChangesAsync();
            
            var counter = 1;
            var quizQuestions = await _context.QuizQuestion.Where(q => q.QuizId == quizId).ToListAsync();
            foreach (var question in quizQuestions)
            {
                question.Number = counter;
                _context.QuizQuestion.Update(question);
                await _context.SaveChangesAsync();
                counter++;
            }

            return NoContent();
        }
    }
}
