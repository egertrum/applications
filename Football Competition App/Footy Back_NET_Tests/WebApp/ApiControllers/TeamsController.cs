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
    /// API Controller for Teams.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeamsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private TeamMapper _teamMapper;

        /// <summary>
        /// Constructor for Team API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public TeamsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            IMapper mapper1 = mapper;
            _teamMapper = new TeamMapper(mapper1);
        }
        
        /// <summary>
        /// Gets all of the teams from database.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Team entity type</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.Team?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Team>>> GetTeams()
        {
            return Ok((await _bll.Teams.GetAllAsync()).Select(x => _teamMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets all of the competitions from database that belong to currently logged in user.
        /// Allowed for all types of authorized users.
        /// </summary> 
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Competition entity type</returns>
        [HttpGet("/api/v{version:apiVersion}/Teams/myTeams")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.Team?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Team>>> GetMyTeams()
        {
            return Ok((await _bll.Teams.GetAllAsync(User.GetUserId()!.Value)).Select(x => _teamMapper.Map(x)));
        }

        /// <summary>
        /// Gets the team of which id was given in the method.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <param name="id">Id of team which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Team entity type.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Team), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.Team>> GetTeam(Guid id)
        {
            var team = _teamMapper.Map(await _bll.Teams.FirstOrDefaultAsync(id));
            if (team == null) return NotFound();
            return team;
        }
        
        /// <summary>
        /// Checks if the team belongs to current user or not.
        /// </summary>
        /// <param name="id"> Id of team which details are needed.</param>
        /// <returns>True if belongs to user and false if it does not.</returns>
        [HttpGet("/api/v{version:apiVersion}/Teams/belongsToUser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> TeamBelongsToUser(Guid id)
        {
            var team = _teamMapper.Map(await _bll.Teams.FirstOrDefaultAsync(id));
            if (team == null) return NotFound();
            var belongsToUser = team.AppUserId == User.GetUserId();
            return Ok(belongsToUser);
        }

        /// <summary>
        /// Edits the Team which id was given.
        /// </summary>
        /// <param name="id">Id of the team that is going to be edited</param>
        /// <param name="team">PublicApi version 1 team entity type with edited values.</param>
        /// <returns>NoContent if everything went successfully.</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutTeam(Guid id, PublicApi.DTO.v1.Team team)
        {
            if (id != team.Id) return BadRequest();
            _bll.Teams.Update(_teamMapper.Map(team)!);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates a new Team entity.
        /// </summary>
        /// <param name="team">New Team that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new team.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Team), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Team>> PostTeam(PublicApi.DTO.v1.Team team)
        {
            var addTeam = _teamMapper.Map(team);
            addTeam!.AppUserId = User.GetUserId()!.Value;
            _bll.Teams.Add(addTeam!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        /// <summary>
        /// Tries to delete the team which id was given. For admins.
        /// </summary>
        /// <param name="id">Team's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var team = await _bll.Teams.FirstOrDefaultAsync(id);
            if (team == null) return NotFound();

            try
            {
                _bll.Teams.Remove(team);
                await _bll.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
