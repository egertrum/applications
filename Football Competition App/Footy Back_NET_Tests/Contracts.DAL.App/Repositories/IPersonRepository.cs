using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IPersonRepository: IBaseRepository<DALAppDTO.Person>, IPersonRepositoryCustom<DALAppDTO.Person>
    {
    }

    public interface IPersonRepositoryCustom<TEntity>
    {
        Task<TEntity?> FindByIdentificationCode(string idCode, Guid userId, bool noTracking = true);
        
        Task<bool> ExistsByIdentificationCode(string idCode, Guid userId, bool noTracking = true);

        Task<TEntity?> FirstOrDefaultAsyncWithTeams(Guid id, bool noTracking = true);
    }
    
}