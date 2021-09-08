using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface IRegistrationService: IBaseEntityService<BLLAppDTO.Registration, DALAppDTO.Registration>, IRegistrationRepositoryCustom<BLLAppDTO.Registration>
    {
        
    }
}