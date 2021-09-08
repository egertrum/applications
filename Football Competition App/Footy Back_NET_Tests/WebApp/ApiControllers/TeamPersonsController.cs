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
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for Team Persons.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TeamPersonsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private TeamPersonMapper _teamPersonMapper;

        /// <summary>
        /// Constructor for Team Persons API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public TeamPersonsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _teamPersonMapper = new TeamPersonMapper(mapper);
        }

        /// <summary>
        /// Gets all of the team persons from database.
        /// For admin only.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Team Person entity type.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.TeamPerson?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TeamPerson>>> GetTeamPersons()
        {
            return Ok((await _bll.TeamPersons.GetAllAsync()).Select(x => _teamPersonMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets all of the team persons from database by team id.
        /// </summary>
        /// /// <param name="teamId">Team id of which details are needed.</param>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Team Person entity type.</returns>
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/TeamPersons/team")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.TeamPerson?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.TeamPerson>>> GetTeamPersonsByTeamId(Guid teamId)
        {
            return Ok((await _bll.TeamPersons.GetAllWithTeamId(teamId)).Select(x => _teamPersonMapper.Map(x))!);
        }
        
        /// <summary>
        /// Gets the team person of which id was given in the method.
        /// </summary>
        /// <param name="id">Id of team person which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Team Person entity type.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PublicApi.DTO.v1.TeamPerson>> GetTeamPerson(Guid id)
        {
            var teamPerson = _teamPersonMapper.Map(await _bll.TeamPersons.FirstOrDefaultAsync(id));
            if (teamPerson == null) return NotFound();

            return teamPerson;
        }

        /// <summary>
        /// Creates a new Team Person entity.
        /// </summary>
        /// <param name="teamPerson">New Team Person that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new team person.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.TeamPerson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PublicApi.DTO.v1.TeamPerson>> PostTeamPerson(PublicApi.DTO.v1.TeamPerson teamPerson)
        {
            
            var bllTeamPerson = _teamPersonMapper.Map(teamPerson);
            var allowed = await _bll.TeamPersons.TeamAndPersonBelongToUser(bllTeamPerson!, User.GetUserId()!.Value);
            if (!allowed) return NotFound("Not allowed");
            var addedPerson = await _bll.TeamPersons.AddIfPossible(bllTeamPerson!);
            if (addedPerson == null) return BadRequest("Already added");

            return CreatedAtAction("GetTeamPerson", new { id = teamPerson.Id }, teamPerson);
        }

        /// <summary>
        /// Deletes the team person if the team and person belong to current user.
        /// </summary>
        /// <param name="id">Team person's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteTeamPerson(Guid id)
        {
            var bllTeamPerson = await _bll.TeamPersons.FirstOrDefaultAsync(id);
            var allowed = await _bll.TeamPersons.TeamAndPersonBelongToUser(bllTeamPerson!, User.GetUserId()!.Value);
            if (!allowed) return BadRequest();

            _bll.TeamPersons.Remove(bllTeamPerson);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
