using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for Reports.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReportsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UserManager<Domain.App.Identity.AppUser> _userManager;
        private readonly ReportMapper _reportMapper;

        /// <summary>
        /// Constructor for API Reports Controller
        /// </summary>
        /// <param name="bll">Business layer</param>
        /// <param name="userManager">To manage logged in user.</param>
        /// <param name="mapper">To map entities between different layers.</param>
        public ReportsController(IAppBLL bll, UserManager<Domain.App.Identity.AppUser> userManager, IMapper mapper)
        {
            _bll = bll;
            _userManager = userManager;
            _reportMapper = new ReportMapper(mapper);
        }
        
        /// <summary>
        /// Gets all of the reports from database.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Report entity type</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Report?>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Report?>>> GetReports()
        {
            return Ok((await _bll.Reports.GetAllAsync())
                .Select(x => _reportMapper.Map(x)));
        }
        
        
        /// <summary>
        /// Gets the Report of which id was given in the method.
        /// </summary>
        /// <param name="id"> Id of Report which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Report entity type</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Report), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Report>> GetReport(Guid id)
        {
            var report = _reportMapper.Map(await _bll.Reports.FirstOrDefaultAsync(id));
            if (report == null) return NotFound();
            return report;
        }
        
        /// <summary>
        /// Creating a new Report entity.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <param name="report">New Report that is going to be added to database.</param>
        /// <returns>CreatedAtAction which returns new Report.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Report), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Report>> PostReport(Report report)
        {
            var user = await _userManager.GetUserAsync(User);
            var bllReport = _reportMapper.Map(report);
            var bllWithSubmitter = _bll.Reports.AddSubmitter(bllReport!, user);
            var addedReport = _bll.Reports.Add(bllWithSubmitter!);
            await _bll.SaveChangesAsync();

            var returnReport = _reportMapper.Map(addedReport);

            return CreatedAtAction("GetReport", new { id = returnReport!.Id }, returnReport);
        }
        
        /// <summary>
        /// Deleting the Report which id was given.
        /// </summary>
        /// <param name="id">Report that is going to be deleted.</param>
        /// <returns>NoContent if valid and else NotFound.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            var report = await _bll.Reports.FirstOrDefaultAsync(id);
            if (report == null) return NotFound();

            _bll.Reports.Remove(report);
            await _bll.SaveChangesAsync();

            return NoContent();
        }
    }
}
