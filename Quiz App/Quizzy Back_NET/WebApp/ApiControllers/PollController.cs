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
    public class PollController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PollMapper _pollMapper;

        public PollController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _pollMapper = new PollMapper(mapper);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DTO.App.V1.Poll>>> GetPolls()
        {
            return Ok((await _context.Poll.ToListAsync()).Select(x => _pollMapper.Map(x)));
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.Poll>> GetPoll(Guid id)
        {
            var poll = _pollMapper.Map(await _context.Poll.FindAsync(id));

            if (poll == null)
            {
                return NotFound();
            }

            return poll;
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPoll(Guid id, DTO.App.V1.Poll poll)
        {
            if (id != poll.Id) return BadRequest();
            _context.Poll.Update(_pollMapper.Map(poll)!);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DTO.App.V1.Poll>> PostPoll(DTO.App.V1.Poll poll)
        {
            _context.Poll.Add(_pollMapper.Map(poll)!);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPoll", new { id = poll.Id }, poll);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePoll(Guid id)
        {
            var poll = await _context.Poll.FindAsync(id);
            if (poll == null)
            {
                return NotFound();
            }

            try
            {
                _context.Poll.Remove(poll);
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
