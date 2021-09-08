using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IReportRepository: IBaseRepository<DALAppDTO.Report>, IReportRepositoryCustom<DALAppDTO.Report>
    {
    }

    public interface IReportRepositoryCustom<TEntity>
    {
    }
}