using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Domain.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for Person roles.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class PersonRolesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private RoleMapper _personRoleMapper;

        /// <summary>
        /// Constructor for Person roles API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public PersonRolesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _personRoleMapper = new RoleMapper(mapper);
        }

        /// <summary>
        /// Gets all of the person roles from database.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Role entity type</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.Role?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Role>>> GetPersonRoles()
        {
            return Ok((await _bll.Roles.GetAllAsync()).Select(x => _personRoleMapper.Map(x)));
        }

        /// <summary>
        /// Gets the person role of which id was given in the method.
        /// </summary>
        /// <param name="id">Id of person role which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Role entity type.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Role), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.Role>> GetRole(Guid id)
        {
            var role = _personRoleMapper.Map(await _bll.Roles.FirstOrDefaultAsync(id));
            if (role == null) return NotFound();

            return role;
        }

        /// <summary>
        /// Edits the Person Role which id was given.
        /// </summary>
        /// <param name="id">Id of the person role that is going to be edited.</param>
        /// <param name="role">PublicApi version 1 role entity type with edited values.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutRole(Guid id, PublicApi.DTO.v1.Role role)
        {
            if (id != role.Id) return BadRequest();
            _bll.Roles.Update(_personRoleMapper.Map(role)!);
            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        /// <summary>
        /// Creates a new Role entity.
        /// </summary>
        /// <param name="role">New Role that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new role.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Role), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Role>> PostRole(PublicApi.DTO.v1.Role role)
        {
            _bll.Roles.Add(_personRoleMapper.Map(role)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        /// <summary>
        /// Deletes the person role which id was given.
        /// </summary>
        /// <param name="id">Role's id that is going to be deleted.</param>
        /// <returns>NoContent if successful.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _bll.Roles.FirstOrDefaultAsync(id);
            if (role == null) return NotFound();

            _bll.Roles.Remove(role);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
