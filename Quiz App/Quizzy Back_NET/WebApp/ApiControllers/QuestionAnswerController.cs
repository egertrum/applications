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
using Domain.Base;
using DTO.App.V1.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly QuestionAnswerMapper _questionAnswerMapper;

        public QuestionAnswerController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _questionAnswerMapper = new QuestionAnswerMapper(mapper);
        }
        
        [HttpGet("/api/v{version:apiVersion}/QuestionAnswers")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.QuestionAnswer>>> GetQuestionAnswers(Guid? id)
        {
            var appDbContext = _context.QuestionAnswer
                .Include(q => q.Question);
            var returnList = id != null
                ? await appDbContext
                    .Where(a => a.QuestionId == id)
                    .OrderBy(q => Guid.NewGuid())
                    .ToListAsync()
                : await appDbContext.OrderBy(q => q.Question!.Value).ToListAsync();
            
            return Ok(returnList.Select(x => _questionAnswerMapper.Map(x)));
        }
        
        [HttpGet("/api/v{version:apiVersion}/QuestionAnswers/All")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.QuestionAnswer>>> GetAllQuestionAnswers(Guid id)
        {
            var appDbContext = _context.QuestionAnswer
                .Include(q => q.Question);
            var returnList = await appDbContext
                .Where(a => a.QuestionId == id)
                .OrderBy(q => q.Value)
                .ToListAsync();
            
            return Ok(returnList.Select(x => _questionAnswerMapper.Map(x)));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.QuestionAnswer>> GetQuestionAnswer(Guid id)
        {
            var questionAnswer = _questionAnswerMapper.Map(await _context.QuestionAnswer
                .Include(q => q.Question)
                .FirstOrDefaultAsync(q => q.Id == id));

            if (questionAnswer == null)
            {
                return NotFound();
            }

            return questionAnswer;
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutQuestionAnswer(Guid id, DTO.App.V1.QuestionAnswer questionAnswer)
        {
            if (id != questionAnswer.Id) return BadRequest();
            _context.QuestionAnswer.Update(_questionAnswerMapper.Map(questionAnswer)!);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.QuestionAnswer>> PostQuestionAnswer(DTO.App.V1.QuestionAnswer questionAnswer)
        {
            var question = await _context.Question.FindAsync(questionAnswer.QuestionId);
            if (question.QuestionType == EQuestionType.Poll)
            {
                questionAnswer.True = true;
            }
            await _context.QuestionAnswer.AddAsync(_questionAnswerMapper.Map(questionAnswer)!);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionAnswer", new { id = questionAnswer.Id }, questionAnswer);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionAnswer(Guid id)
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

            return NoContent();
        }
    }
}
