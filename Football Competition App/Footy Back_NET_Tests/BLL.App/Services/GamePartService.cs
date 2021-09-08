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
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;


namespace BLL.App.Services
{
    public class GamePartService: BaseEntityService<IAppUnitOfWork, IGamePartRepository, BLLAppDTO.GamePart, DALAppDTO.GamePart>, IGamePartService
    {
        public GamePartService(IAppUnitOfWork serviceUow, IGamePartRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new GamePartMapper(mapper))
        {
        }

        public async Task<IEnumerable<BLLAppDTO.GamePart>> GetAllByGameId(Guid gameId)
        {
            return (await ServiceRepository.GetAllByGameId(gameId)).Select(x => Mapper.Map(x)!);
        }

        public async Task<int> GetNormalTimeLength(Guid gameId)
        {
            return await ServiceRepository.GetNormalTimeLength(gameId);
        }

        public async Task<int?> GetExtraTimeLength(Guid gameId)
        {
            return await ServiceRepository.GetExtraTimeLength(gameId);
        }
    }

}