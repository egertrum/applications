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
    public class GameTypeService: BaseEntityService<IAppUnitOfWork, IGameTypeRepository, BLLAppDTO.GameType, DALAppDTO.GameType>, IGameTypeService
    {
        public GameTypeService(IAppUnitOfWork serviceUow, IGameTypeRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new GameTypeMapper(mapper))
        {
        }
    }
}