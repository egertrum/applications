using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ICountryRepository: IBaseRepository<DALAppDTO.Country>, ICountryRepositoryCustom<DALAppDTO.Country>
    {
    }

    public interface ICountryRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllWithCountsAsync(bool noTracking = true);
    }
}