using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Domain.App.Identity;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface IGameService: IBaseEntityService<BLLAppDTO.Game, DALAppDTO.Game>, IGameRepositoryCustom<BLLAppDTO.Game>
    {
        public Task AddGameParts(BLLAppDTO.Game game, BLLAppDTO.GameLength gameLength , bool noTracking = true);

        public Task<bool> UpdateGameAndGameParts(BLLAppDTO.Game game, BLLAppDTO.GameLength gameLength,
            bool noTracking = true);

        public Task DeleteGameParts(Guid gameId);
    }
}