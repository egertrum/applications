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
    public class QuestionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly QuestionMapper _questionMapper;

        public QuestionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _questionMapper = new QuestionMapper(mapper);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.Question>>> GetQuestions()
        {
            return Ok((await _context.Question.ToListAsync()).Select(x => _questionMapper.Map(x)));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.Question>> GetQuestion(Guid id)
        {
            var question = _questionMapper.Map(await _context.Question.FindAsync(id));

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutQuestion(Guid id, DTO.App.V1.Question question)
        {
            var post = _questionMapper.Map(question)!;
            post.QuestionType = question.QuestionType == "Poll" ? EQuestionType.Poll : EQuestionType.Quiz;
            if (id != question.Id) return BadRequest();
            _context.Question.Update(post);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.Question>> PostQuestion(DTO.App.V1.Question question)
        {
            var post = _questionMapper.Map(question)!;
            post.QuestionType = question.QuestionType == "Poll" ? EQuestionType.Poll : EQuestionType.Quiz;
            await _context.Question.AddAsync(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            try
            {
                _context.Question.Remove(question);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
