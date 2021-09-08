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
    public class QuizController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly QuizMapper _quizMapper;

        public QuizController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _quizMapper = new QuizMapper(mapper);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.Quiz>>> GetQuizzes()
        {
            return Ok((await _context.Quiz.ToListAsync()).Select(x => _quizMapper.Map(x)));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.Quiz>> GetQuiz(Guid id)
        {
            var quiz = _quizMapper.Map(await _context.Quiz.FindAsync(id));

            if (quiz == null)
            {
                return NotFound();
            }

            return quiz;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutQuiz(Guid id, DTO.App.V1.Quiz quiz)
        {
            if (id != quiz.Id) return BadRequest();
            _context.Quiz.Update(_quizMapper.Map(quiz)!);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.Quiz>> PostQuiz(DTO.App.V1.Quiz quiz)
        {
            quiz.MaxPoints = 0;
            await _context.Quiz.AddAsync(_quizMapper.Map(quiz)!);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.Id }, quiz);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteQuiz(Guid id)
        {
            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            try
            {
                _context.Quiz.Remove(quiz);
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
