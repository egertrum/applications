using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PublicApi.DTO.v1.Mappers;
using Competition = PublicApi.DTO.v1.Competition;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for Competitions.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CompetitionsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly CompetitionMapper _competitionMapper;

        /// <summary>
        /// Constructor for Competition API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public CompetitionsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _competitionMapper = new CompetitionMapper(mapper);
        }
        
        /// <summary>
        /// Gets all of the competitions from database.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition entity type</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Competition?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Competition?>>> GetCompetitions()
        {
            return Ok((await _bll.Competitions.GetAllAsync())
                .Select(x => _competitionMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets all of the competitions from database by search parameters.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition entity type</returns>
        [HttpGet("/api/v{version:apiVersion}/Competitions/search")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Competition?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Competition?>>> GetCompetitionsBySearch(Guid? countryId, string? name, DateTime? startDate)
        {
            var searchCompetition = new Competition()
            {
                CountryId = countryId ?? Guid.Empty,
                StartDate = startDate ?? DateTime.MinValue,
                Name = name ?? ""
            };
            return Ok((await _bll.Competitions.GetAllAsyncWithSearch(_competitionMapper.Map(searchCompetition)!))
                .Select(x => _competitionMapper.Map(x)));
        }

        /// <summary>
        /// Gets all of the competitions from database that belong to currently logged in user.
        /// Allowed for all types of authorized users.
        /// </summary> 
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition entity type</returns>
        [HttpGet("/api/v{version:apiVersion}/Competitions/myCompetitions")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Competition?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Competition>>> GetMyCompetitions()
        {
            return Ok((await _bll.Competitions.GetAllAsync(User.GetUserId()!.Value))
                .Select(x => _competitionMapper.Map(x)));
        }

        
        /// <summary>
        /// Gets the competition of which id was given in the method.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <param name="id">Id of competition which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Competition entity type.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Competition), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Competition>> GetCompetition(Guid id)
        {
            var competition = _competitionMapper.Map(await _bll.Competitions.FirstOrDefaultAsync(id));
            if (competition == null) return NotFound();
            return competition;
        }
        
        /// <summary>
        /// Checks if the competition belongs to current user or not.
        /// Allowed for all types of authorized users.
        /// </summary>
        /// <param name="id"> Id of competition which details are needed.</param>
        /// <returns>True if belongs to user and false if it does not.</returns>
        [HttpGet("/api/v{version:apiVersion}/Competitions/userCompetitions")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> BelongsToUser(Guid id)
        {
            var competition = await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id);
            return competition;
        }
        
        /// <summary>
        /// Edits the Competition which id was given.
        /// </summary>
        /// <param name="id">Id of the competition that is going to be edited</param>
        /// <param name="competition">PublicApi version 1 competition entity type with edited values.</param>
        /// <returns>NoContent if everything went successfully.</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutCompetition(Guid id, Competition competition)
        {
            if (id != competition.Id) return BadRequest();

            if (!User.IsInRole("Admin") && !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id))
                return BadRequest();

            var bllComp = _competitionMapper.Map(competition);
            _bll.Competitions.Update(bllComp!);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
        
        /// <summary>
        /// Creates a new Competition entity and makes a registration for the competition with user id.
        /// </summary>
        /// <param name="competition">New Competition that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new competition.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Competition), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
            var bllComp = _competitionMapper.Map(competition);
            var addedComp = _bll.Competitions.Add(bllComp!);
            
            var userId = User.GetUserId()!.Value;
            await _bll.Competitions.MakeRegistration(userId, addedComp.Id);
            await _bll.SaveChangesAsync();

            var returnComp = _competitionMapper.Map(addedComp);

            return CreatedAtAction("GetCompetition", new {id = returnComp!.Id}, returnComp);
        }
        
        /// <summary>
        /// Tries to delete the competition which id was given.
        /// </summary>
        /// <param name="id">Competition's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCompetition(Guid id)
        {
            if (!User.IsInRole("Admin") && !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, id))
                return BadRequest();
            
            var competition = await _bll.Competitions.FirstOrDefaultAsync(id);
            if (competition == null) return NotFound();
            
            var reg = await _bll.Registrations.GetByCompetitionId(id);
            try
            {
                _bll.Registrations.Remove(reg);
                _bll.Competitions.Remove(competition);
                await _bll.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}