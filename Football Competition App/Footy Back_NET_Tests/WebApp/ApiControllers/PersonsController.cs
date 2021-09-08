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
    /// API Controller for Persons.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private PersonMapper _personMapper;

        /// <summary>
        /// Constructor for Persons API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public PersonsController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _personMapper = new PersonMapper(mapper);
        }

        /// <summary>
        /// Gets all of the persons from database that belong to current user or
        /// all of the persons if user role is Admin.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Person entity type</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.Person?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Person>>> GetPersons()
        {
            var persons = User.IsInRole("Admin")
                ? (await _bll.Persons.GetAllAsync()).Select(x => _personMapper.Map(x)!)
                : (await _bll.Persons.GetAllAsync(User.GetUserId()!.Value)).Select(x =>
                    _personMapper.Map(x)!);
            
            return Ok(persons);
        }

        /// <summary>
        /// Gets the person of which id was given in the method.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <param name="id">Id of person which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Person entity type.</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.Person>> GetPerson(Guid id)
        {
            var person = _personMapper.Map(await _bll.Persons.FirstOrDefaultAsyncWithTeams(id));
            if (person == null) return NotFound();
            return person;
        }
        
        /// <summary>
        /// Checks if the person belongs to current user or not.
        /// </summary>
        /// <param name="id"> Id of person which details are needed.</param>
        /// <returns>True if belongs to user and false if it does not.</returns>
        [HttpGet("/api/v{version:apiVersion}/Persons/belongsToUser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> PersonBelongsToUser(Guid id)
        {
            var person = await _bll.Persons.FirstOrDefaultAsync(id);
            if (person == null) return NotFound();
            var belongsToUser = person.AppUserId == User.GetUserId();
            return Ok(belongsToUser);
        }

        /// <summary>
        /// Edits the Person which id was given.
        /// </summary>
        /// <param name="id">Id of the person that is going to be edited</param>
        /// <param name="person">PublicApi version 1 person entity type with edited values.</param>
        /// <returns>NoContent if everything went successfully.</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutPerson(Guid id, PublicApi.DTO.v1.Person person)
        {
            if (id != person.Id) return BadRequest();
            
            var personFromDb = _personMapper.Map(await _bll.Persons.FirstOrDefaultAsync(id, User.GetUserId()!.Value));
            if (personFromDb == null) return NotFound();

            _bll.Persons.Update(_personMapper.Map(person)!);
            await _bll.SaveChangesAsync();
            
            return NoContent();
        }

        /// <summary>
        /// Creates a new Person entity.
        /// </summary>
        /// <param name="person">New Person that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new person.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Person>> PostPerson(PublicApi.DTO.v1.Person person)
        {
            var alreadyEntered =
                await _bll.Persons.ExistsByIdentificationCode(person.IdentificationCode, User.GetUserId()!.Value);
            if (alreadyEntered)
                return BadRequest();
            person.AppUserId = User.GetUserId()!.Value;
            
            var addedPerson = _bll.Persons.Add(_personMapper.Map(person)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = addedPerson.Id }, addedPerson);
        }

        /// <summary>
        /// Tries to delete the person which id was given.
        /// </summary>
        /// <param name="id">Person's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            var person = await _bll.Persons.FirstOrDefaultAsync(id);
            if (person == null) return NotFound();

            try
            {
                _bll.Persons.Remove(person);
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
