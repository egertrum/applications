using System;
using System.Collections.Generic;
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
    public class GamePartTypeService: BaseEntityService<IAppUnitOfWork, IGamePartTypeRepository, BLLAppDTO.GamePartType, DALAppDTO.GamePartType>, IGamePartTypeService
    {
        public GamePartTypeService(IAppUnitOfWork serviceUow, IGamePartTypeRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new GamePartTypeMapper(mapper))
        {
        }

        public async Task<Guid> FindIdByShort(EGamePartType shortName)
        {
            return await ServiceRepository.FindIdByShort(shortName);
        }
    }

}