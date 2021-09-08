using System;
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
    public class GamePersonService: BaseEntityService<IAppUnitOfWork, IGamePersonRepository, BLLAppDTO.GamePerson, DALAppDTO.GamePerson>, IGamePersonService
    {
        public GamePersonService(IAppUnitOfWork serviceUow, IGamePersonRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new GamePersonMapper(mapper))
        {
        }

        public int GetGamesCountForPlayer(Guid personId)
        {
            return ServiceRepository.GetGamesCountForPlayer(personId);
        }
    }

}