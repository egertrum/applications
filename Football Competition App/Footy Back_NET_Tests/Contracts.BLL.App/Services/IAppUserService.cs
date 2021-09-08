using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface IAppUserService: IAppUserRepository<BLLAppDTO.Identity.AppUser>
    {
        
    }
}