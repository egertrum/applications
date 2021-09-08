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
    public class RoleService: BaseEntityService<IAppUnitOfWork, IRoleRepository, BLLAppDTO.Role, DALAppDTO.Role>, IRoleService
    {
        public RoleService(IAppUnitOfWork serviceUow, IRoleRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new RoleMapper(mapper))
        {
        }
    }

}