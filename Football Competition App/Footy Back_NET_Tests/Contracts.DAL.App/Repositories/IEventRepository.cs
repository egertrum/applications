using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IEventRepository: IBaseRepository<DALAppDTO.Event>, IEventRepositoryCustom<DALAppDTO.Event>
    {
    }

    public interface IEventRepositoryCustom<TEntity>
    {
    }
    
}