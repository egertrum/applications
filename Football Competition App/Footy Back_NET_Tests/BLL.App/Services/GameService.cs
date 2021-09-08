using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Domain.Base;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;


namespace BLL.App.Services
{
    public class GameService : BaseEntityService<IAppUnitOfWork, IGameRepository, BLLAppDTO.Game, DALAppDTO.Game>,
        IGameService
    {
        public GameService(IAppUnitOfWork serviceUow, IGameRepository serviceRepository, IMapper mapper) : base(
            serviceUow, serviceRepository, new GameMapper(mapper))
        {
        }

        public async Task AddGameParts(BLLAppDTO.Game game, BLLAppDTO.GameLength gameLength, bool noTracking = true)
        {
            if (game.HomeScore != null && game.AwayScore != null)
            {
                game = AddWinnerToGame(game);
            }

            await AddHalves(game.Id, gameLength.HalfLength);

            if (gameLength.ExtraTimeHalfLength != null)
            {
                await AddExtraTime(game.Id, gameLength.ExtraTimeHalfLength.Value);
            }

            var gameType = await ServiceUow.GameTypes.FirstOrDefaultAsync(game.GameTypeId);
            if (gameType!.Calling == EGameTypeCalling.Penalties)
            {
                await AddPenalties(game.Id);
            }

            await ServiceUow.SaveChangesAsync();
        }

        private async Task AddHalves(Guid gameId, int halfLength)
        {
            var firstHalf = await MakeGamePart(gameId, EGamePartType.FirstHalf, halfLength);
            var secondHalf = await MakeGamePart(gameId, EGamePartType.SecondHalf, halfLength);
            ServiceUow.GameParts.Add(firstHalf);
            ServiceUow.GameParts.Add(secondHalf);
        }
        
        private static BLLAppDTO.Game AddWinnerToGame(BLLAppDTO.Game game)
        {
            if (game.HomeScore > game.AwayScore)
            {
                game.IdOfGameWinner = game.HomeId;
            }
            else if (game.AwayScore > game.HomeScore)
            {
                game.IdOfGameWinner = game.AwayId;
            }

            return game;
        }

        public async Task<bool> UpdateGameAndGameParts(BLLAppDTO.Game game, BLLAppDTO.GameLength gameLength,
            bool noTracking = true)
        {
            var gameFromDb = Mapper.Map(await ServiceUow.Games.FirstOrDefaultAsync(game.Id));
            if (gameFromDb == null)
                return false;

            if (gameFromDb.GameTypeId != game.GameTypeId)
            {
                await DeleteGameParts(game.Id);
                await AddGameParts(game, gameLength);
            }
            else
            {
                var gameParts = await ServiceUow.GameParts.GetAllByGameId(game.Id);
                foreach (var gamePart in gameParts)
                {
                    await UpdateGamePart(gamePart, gameLength);
                }
            }

            if (game.HomeScore != null && game.AwayScore != null)
            {
                game = AddWinnerToGame(game);
            }

            var updatedGame = ServiceUow.Games.Update(Mapper.Map(game)!);
            await ServiceUow.SaveChangesAsync();

            return true;
        }
        
        private async Task UpdateGamePart(DALAppDTO.GamePart gamePart, BLLAppDTO.GameLength gameLength)
        {
            var gamePartType = await ServiceUow.GamePartTypes.FirstOrDefaultAsync(gamePart.GamePartTypeId);
            if (gamePartType!.Short == EGamePartType.FirstHalf ||
                gamePartType.Short == EGamePartType.SecondHalf)
            {
                gamePart.Length = gameLength.HalfLength;
                ServiceUow.GameParts.Update(gamePart);
            }
            else if (gamePartType!.Short == EGamePartType.ExtraTimeFirstHalf ||
                     gamePartType.Short == EGamePartType.ExtraTimeSecondHalf)
            {
                gamePart.Length = gameLength.ExtraTimeHalfLength!.Value;
                ServiceUow.GameParts.Update(gamePart);
            }

            await ServiceUow.SaveChangesAsync();
        }

        public async Task DeleteGameParts(Guid gameId)
        {
            var gameParts = await ServiceUow.GameParts.GetAllByGameId(gameId);

            foreach (var gamePart in gameParts)
            {
                ServiceUow.GameParts.Remove(gamePart);
            }

            await ServiceUow.SaveChangesAsync();
        }

        private async Task AddExtraTime(Guid gameId, int halfLength)
        {
            var extraFirstHalf = await MakeGamePart(gameId, EGamePartType.ExtraTimeFirstHalf, halfLength);
            var extraSecondHalf = await MakeGamePart(gameId, EGamePartType.ExtraTimeSecondHalf, halfLength);
            ServiceUow.GameParts.Add(extraFirstHalf);
            ServiceUow.GameParts.Add(extraSecondHalf);
        }

        private async Task AddPenalties(Guid gameId)
        {
            var penalties = await MakeGamePart(gameId, EGamePartType.Penalties, 0);
            ServiceUow.GameParts.Add(penalties);
        }

        private async Task<DAL.App.DTO.GamePart> MakeGamePart(Guid gameId, EGamePartType typeShortName, int halfLength)
        {
            var gamePart = new DAL.App.DTO.GamePart()
            {
                GameId = gameId,
                GamePartTypeId = await ServiceUow.GamePartTypes.FindIdByShort(typeShortName),
                Length = halfLength,
                AdditionalTime = null
            };
            return gamePart;
        }

        public async Task<IEnumerable<BLLAppDTO.Game>> GetAllUserGames(Guid userId)
        {
            return (await ServiceRepository.GetAllUserGames(userId)).Select(x => Mapper.Map(x)!);
        }

        public async Task<IEnumerable<BLLAppDTO.Game>> GetAllOrganiserGames(Guid userId)
        {
            return (await ServiceRepository.GetAllOrganiserGames(userId)).Select(x => Mapper.Map(x)!);
        }

        public async Task<IEnumerable<BLLAppDTO.Game>> GetAllByCompetitionId(Guid competitionId)
        {
            return (await ServiceRepository.GetAllByCompetitionId(competitionId)).Select(x => Mapper.Map(x)!);
        }
    }
}