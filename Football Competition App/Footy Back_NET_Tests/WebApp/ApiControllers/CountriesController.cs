using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for Countries.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CountriesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly CountryMapper _countryMapper;

        /// <summary>
        /// Constructor for Country API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public CountriesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            _countryMapper = new CountryMapper(mapper);
        }
        
        /// <summary>
        /// Gets all of the countries from database.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Country entity type</returns>
        [AllowAnonymous]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<PublicApi.DTO.v1.Country?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<PublicApi.DTO.v1.Country>>> GetCountries()
        {
            return Ok((await _bll.Countries.GetAllAsync()).Select(x => _countryMapper.Map(x)));
        }
        
        /// <summary>
        /// Gets the country of which id was given in the method.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <param name="id">Id of country which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Country entity type</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Country), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.Country>> GetCountry(Guid id)
        {
            var country = _countryMapper.Map(await _bll.Countries.FirstOrDefaultAsync(id));
            if (country == null) return NotFound();
            return country;
        }
        
        /// <summary>
        /// Edits the Country which id was given.
        /// For admins only.
        /// </summary>
        /// <param name="id">Id of the country that is going to be edited</param>
        /// <param name="country">PublicApi version 1 country entity type with edited values.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutCountry(Guid id, PublicApi.DTO.v1.Country country)
        {
            if (id != country.Id) return BadRequest();
            _bll.Countries.Update(_countryMapper.Map(country)!);
            await _bll.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Creates a new Country entity.
        /// For admins only.
        /// </summary>
        /// <param name="country">New Country entity that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new country.</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Country), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PublicApi.DTO.v1.Country>> PostCountry(PublicApi.DTO.v1.Country country)
        {
            _bll.Countries.Add(_countryMapper.Map(country)!);
            await _bll.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        /// <summary>
        /// Deletes the country which id was given.
        /// For admins only.
        /// </summary>
        /// <param name="id">Country's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {
            var country = await _bll.Countries.FirstOrDefaultAsync(id);
            if (country == null) return NotFound();

            _bll.Countries.Remove(country);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
