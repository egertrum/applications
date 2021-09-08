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
    public class PollQuestionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PollQuestionMapper _pollQuestionMapper;
        private readonly QuestionMapper _questionMapper;

        public PollQuestionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _pollQuestionMapper = new PollQuestionMapper(mapper);
            _questionMapper = new QuestionMapper(mapper);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.PollQuestion>>> GetPollQuestions()
        {
            return Ok((await _context.PollQuestion.ToListAsync()).Select(x => _pollQuestionMapper.Map(x)));
        }
        
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/PollQuestion/Max")]
        public ActionResult<int> GetQuizQuestionMaxNumber(Guid pollId)
        {
            return _context.PollQuestion.Count(q => q.PollId == pollId);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpGet("/api/v{version:apiVersion}/PollQuestion/Poll")]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.PollQuestion>>> GetPollQuestionsByPollId(Guid pollId)
        {
            return Ok((await _context.PollQuestion
                .Include(q => q.Question)
                .Where(q => q.PollId == pollId)
                .OrderBy(q => q.Number)
                .ToListAsync()).Select(x => _pollQuestionMapper.Map(x)));
        }
        
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/PollQuestion/Poll/Number")]
        public ActionResult<DTO.App.V1.Question> GetPollQuestionByPollIdAndNumber(Guid pollId, int number)
        {
            var question = (_context.PollQuestion.Include(q => q.Question)
                .First(q => q.PollId == pollId && q.Number == number)).Question!;
            return Ok(_questionMapper.Map(question));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.PollQuestion>> GetPollQuestion(Guid id)
        {
            var pollQuestion = _pollQuestionMapper.Map(await _context.PollQuestion
                .Include(p => p.Poll)
                .Include(p => p.Question)
                .FirstOrDefaultAsync(p=> p.Id == id));

            if (pollQuestion == null)
            {
                return NotFound();
            }

            return pollQuestion;
        }
        
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.PollQuestion>> PostPollQuestion(DTO.App.V1.PollQuestion pollQuestion)
        {
            pollQuestion.Number = _context.PollQuestion.Count(q => q.PollId == pollQuestion.PollId) + 1;
            await _context.PollQuestion.AddAsync(_pollQuestionMapper.Map(pollQuestion)!);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPollQuestion", new { id = pollQuestion.Id }, pollQuestion);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePollQuestion(Guid id)
        {
            var pollQuestion = await _context.PollQuestion.FindAsync(id);

            if (pollQuestion == null)
            {
                return NotFound();
            }
            
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

            return NoContent();
        }
    }
}
