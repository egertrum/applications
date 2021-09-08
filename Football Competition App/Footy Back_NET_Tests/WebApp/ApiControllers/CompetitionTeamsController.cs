using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.App;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for teams at competitions(Competition Teams).
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CompetitionTeamsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private CompetitionTeamMapper _competitionTeamMapper;


        /// <summary>
        /// Constructor for Competition Teams API Controller.
        /// </summary>
        /// <param name="bll">Business layer</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public CompetitionTeamsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            IMapper mapper1 = mapper;
            _competitionTeamMapper = new CompetitionTeamMapper(mapper1);
        }

        // GET: api/CompetitionTeams
        /// <summary>
        /// Get all of the different teams at competitions.
        /// For admin users only.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition Team entity type.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.CompetitionTeam?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.CompetitionTeam>>> GetCompetitionTeams()
        {
            return Ok((await _bll.CompetitionTeams.GetAllAsync()).Select(x => _competitionTeamMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets all of the competition team entities where competition and team are accessible by the competition id.
        /// Allowed for all types of authenticated users. 
        /// </summary>
        /// <param name="competitionId"></param>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition Team entity type.</returns>
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/CompetitionTeams/competition")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.CompetitionTeam?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.CompetitionTeam>>> GetCompetitionTeamsByCompetitionId(Guid competitionId)
        {
            return Ok((await _bll.CompetitionTeams.GetAllByCompetitonId(competitionId))
                .Select(x => _competitionTeamMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets all of the competition team entities for team manager so the team manager
        /// is able to see in which competitions their teams are attending. 
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition Team entity type.</returns>
        [HttpGet("/api/v{version:apiVersion}/CompetitionTeams/teamManager")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.CompetitionTeam?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.CompetitionTeam>>> GetTeamManagerCompetitionTeams()
        {
            return Ok((await _bll.CompetitionTeams.GetAllAsync(User.GetUserId()!.Value))
                .Select(x => _competitionTeamMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets all of the competition team entities for competition organiser so that the organiser
        /// is able to see which teams are attending their competitions. 
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition Team entity type.</returns>
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.CompetitionTeam?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("/api/v{version:apiVersion}/CompetitionTeams/organiser")]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.CompetitionTeam>>> GetOrganiserCompetitionTeams()
        {
            return Ok((await _bll.CompetitionTeams.GetAllOrganiserCompetitions(User.GetUserId()!.Value))
                .Select(x => _competitionTeamMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets the competition of which id was given in the method.
        /// Allowed for all types of authorized users.
        /// </summary>
        /// <param name="id">Id of competition team which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Competition Team entity type</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.CompetitionTeam), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.CompetitionTeam>> GetCompetitionTeam(Guid id)
        {
            var competitionTeam = _competitionTeamMapper.Map(await _bll.CompetitionTeams.FirstOrDefaultAsyncWithEntities(id));
            if (competitionTeam == null) return NotFound();
            return competitionTeam;
        }
        
        /// <summary>
        /// Checks if the user is allowed to make changes to the competition team entity.
        /// </summary>
        /// <param name="id">Id of which competition team details is needed.</param>
        /// <returns>True if valid and false if not allowed</returns>
        [HttpGet("/api/v{version:apiVersion}/CompetitionTeams/allowed")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<bool>> GetAllowedToMakeChanges(Guid id)
        {
            var competitionTeam = await _bll.CompetitionTeams.FirstOrDefaultAsync(id);
            if (competitionTeam == null) return NotFound();
            
            return await _bll.CompetitionTeams.BelongsToUserId(User.GetUserId()!.Value, id) || 
                   await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, competitionTeam.CompetitionId) 
                    || User.IsInRole("Admin");
        }
        
        /// <summary>
        /// Creates a new Competition Team entity adn therefore team is registered to competition.
        /// </summary>
        /// <param name="competitionTeam">Competition team entity that is going to be added.</param>
        /// <returns>CreatedAtAction which returns new competition team.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.CompetitionTeam), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PublicApi.DTO.v1.CompetitionTeam>> PostCompetitionTeam(PublicApi.DTO.v1.CompetitionTeam competitionTeam)
        {
            if (await _bll.CompetitionTeams.TeamExistsAtCompetition(competitionTeam.TeamId,
                competitionTeam.CompetitionId)) return BadRequest();
            if (!await _bll.Teams.BelongsToUserId(User.GetUserId()!.Value, competitionTeam.TeamId))
                return NotFound();
            
            var bllCompTeam = _competitionTeamMapper.Map(competitionTeam);
            _bll.CompetitionTeams.Add(bllCompTeam!);
            await _bll.SaveChangesAsync();
            return CreatedAtAction("GetCompetitionTeam", new { id = competitionTeam.Id }, competitionTeam);
        }
        
        /// <summary>
        /// Deletes the competition team entity if competition or team belongs to user
        /// and therefore the team is removed from the competition.
        /// </summary>
        /// <param name="id">Competition team's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCompetitionTeam(Guid id)
        {
            var competitionTeam = await _bll.CompetitionTeams.FirstOrDefaultAsync(id);
            if (competitionTeam == null) return NotFound();
            
            if (!await _bll.CompetitionTeams.BelongsToUserId(User.GetUserId()!.Value, id) &&
                !await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, competitionTeam.CompetitionId))
            {
                return BadRequest();
            }

            _bll.CompetitionTeams.Remove(competitionTeam);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
