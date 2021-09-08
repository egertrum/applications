using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Domain.Base;
using Extensions.Base;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using Game = PublicApi.DTO.v1.Game;

namespace WebApp.ApiControllers
{
    /// <summary>
    /// API Controller for Games.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class GamesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly GameMapper _gameMapper;
        private readonly GameLengthMapper _gameLengthMapper;
        private readonly GameTypeMapper _gameTypeMapper;

        /// <summary>
        /// Constructor for Games API Controller.
        /// </summary>
        /// <param name="bll">Business layer.</param>
        /// <param name="mapper">To map data transfer objects between different layers.</param>
        public GamesController(IAppBLL bll, IMapper mapper)
        {
            _bll = bll;
            IMapper mapper1 = mapper;
            _gameMapper = new GameMapper(mapper1);
            _gameLengthMapper = new GameLengthMapper(mapper1);
            _gameTypeMapper = new GameTypeMapper(mapper1);
        }

        /// <summary>
        /// Gets all of the games from database.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Game entity type.</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            return Ok((await _bll.Games.GetAllAsync()).Select(x => _gameMapper.Map(x)!));
        }
        
        /// <summary>
        /// Gets all of the games that belong to current user in the case
        /// of being the team manager.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Game entity type.</returns>
        [HttpGet("/api/v{version:apiVersion}/Games/teamManager")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Game>>> GetTeamManagerGames()
        {
            return Ok((await _bll.Games.GetAllUserGames(User.GetUserId()!.Value)).Select(x => _gameMapper.Map(x)!));
        }
        
        /// <summary>
        /// Gets all of the games that belong to current user in the case
        /// of being the competition organiser.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Game entity type.</returns>
        [HttpGet("/api/v{version:apiVersion}/Games/organiser")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<Game>>> GetOrganiserGames()
        {
            return Ok((await _bll.Games.GetAllOrganiserGames(User.GetUserId()!.Value)).Select(x => _gameMapper.Map(x)!));
        }
        
        /// <summary>
        /// Gets all of the games that belong to the competition.
        /// Allowed for all type of authenticated users.
        /// </summary>
        /// <param name="id">Competition id of which games are needed.</param>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of Game entity type.</returns>
        [AllowAnonymous]
        [HttpGet("/api/v{version:apiVersion}/Games/competition")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Game>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesByCompetitionId(Guid id)
        {
            return Ok((await _bll.Games.GetAllByCompetitionId(id)).Select(x => _gameMapper.Map(x)!));
        }
        
        /// <summary>
        /// Gets all of the game types that game can have from the database that are GameType entity type.
        /// </summary>
        /// <returns>List of PublicApi version 1 Data Transfer Objects of GameType entity type.</returns>
        [HttpGet("/api/v{version:apiVersion}/Games/gameTypes")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<GameType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<GameType>>> GetGameTypes()
        {
            return Ok((await _bll.GameTypes.GetAllAsync()).Select(x => _gameTypeMapper.Map(x)!));
        }

        /// <summary>
        /// Gets the game of which id was given in the method.
        /// Allowed for all types of authenticated users.
        /// </summary>
        /// <param name="id">Id of game which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Game entity type.</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Game>> GetGame(Guid id)
        {
            var game = _gameMapper.Map(await _bll.Games.FirstOrDefaultAsync(id));
            if (game == null) return NotFound();

            return game;
        }
        
        /// <summary>
        /// Gets the game and game lengths of which game id was given in the method.
        /// </summary>
        /// <param name="id">Id of game which details are needed.</param>
        /// <returns>PublicApi version 1 Data Transfer Object of Game and GameLength joined entity type.</returns>
        [HttpGet("/api/v{version:apiVersion}/Games/gameAndGameLength")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GameAndGameLength), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameAndGameLength>> GetGameAndGameLength(Guid id)
        {
            var game = _gameMapper.Map(await _bll.Games.FirstOrDefaultAsync(id));
            if (game == null) return NotFound();
            var normalTimeLength = await _bll.GameParts.GetNormalTimeLength(id);
            var extraTimeLength = await _bll.GameParts.GetExtraTimeLength(id);
            var gameAndGameLen = new GameAndGameLength()
            {
                Game = game,
                GameLength = new GameLength()
                {
                    HalfLength = normalTimeLength,
                    ExtraTimeHalfLength = extraTimeLength
                }
            };
            return gameAndGameLen;
        }
        
        /// <summary>
        /// Tries to update the game and game parts by the Game and GameLength values.
        /// Checks if competition where game needs to be updated belongs to user or not.
        /// </summary>
        /// <param name="id">Game entity id.</param>
        /// <param name="gameAndLen">PublicApi version 1 Data Transfer Object of Game and GameLength joined entity type.</param>
        /// <returns>NoContent if updating went successfully.</returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PutGame(Guid id, GameAndGameLength gameAndLen)
        {
            if (id != gameAndLen.Game.Id) return NotFound();

            if (!await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value,
                gameAndLen.Game.CompetitionId) && !User.IsInRole("Admin"))
                return NotFound();

            var gameType = await _bll.GameTypes.FirstOrDefaultAsync(gameAndLen.Game.GameTypeId);
            if (gameType!.Calling != EGameTypeCalling.Normal && gameAndLen.GameLength.ExtraTimeHalfLength == null)
                return BadRequest();

            var bllGame = _gameMapper.Map(gameAndLen.Game);
            var bllGameLength = _gameLengthMapper.Map(gameAndLen.GameLength)!;
            var updated = await _bll.Games.UpdateGameAndGameParts(bllGame!, bllGameLength);
            if (!updated) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Tries to add the game and game parts by the Game and GameLength values.
        /// Checks if competition where game needs to be added belongs to user or not.
        /// </summary>
        /// <param name="gameAndGameLength">PublicApi version 1 Data Transfer Object of Game and GameLength joined entity type.</param>
        /// <returns>NoContent if updating went successfully.</returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(GameAndGameLength), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Game>> PostGame(GameAndGameLength gameAndGameLength)
        {
            var userCompetition =
                await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, gameAndGameLength.Game.CompetitionId);
            if (!userCompetition && !User.IsInRole("Admin")) return NotFound();
            
            var gameType = await _bll.GameTypes.FirstOrDefaultAsync(gameAndGameLength.Game.GameTypeId);
            if (gameType!.Calling != EGameTypeCalling.Normal && gameAndGameLength.GameLength.ExtraTimeHalfLength == null) 
                return BadRequest();
            
            var bllGame = _gameMapper.Map(gameAndGameLength.Game);
            var bllGameLength = _gameLengthMapper.Map(gameAndGameLength.GameLength)!;
            var addedGame = _bll.Games.Add(bllGame!);
            await _bll.SaveChangesAsync();
            await _bll.Games.AddGameParts(addedGame, bllGameLength);
            
            return CreatedAtAction("GetGame", new { id = gameAndGameLength.Game.Id }, gameAndGameLength.Game);
        }
        
        /// <summary>
        /// If competition belongs to User or User role is Admin then deletes the game and game parts
        /// that belong to the game.
        /// </summary>
        /// <param name="id">Game's id that is going to be deleted.</param>
        /// <returns>NoContent if valid and else BadRequest.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(Guid id)
        {
            var game = await _bll.Games.FirstOrDefaultAsync(id);
            if (game == null) return NotFound();
            if (!await _bll.Registrations.CompetitionRegisteredToUser(User.GetUserId()!.Value, game.CompetitionId)
               && !User.IsInRole("Admin"))
            {
                return BadRequest();   
            }
            await _bll.Games.DeleteGameParts(id);
            await _bll.SaveChangesAsync();
            await _bll.Games.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return NoContent();
        }
    }
}
