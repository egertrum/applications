using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Domain.App.Identity;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface IReportService: IBaseEntityService<BLLAppDTO.Report, DALAppDTO.Report>, IReportRepositoryCustom<BLLAppDTO.Report>
    {
        public BLLAppDTO.Report AddSubmitter(BLLAppDTO.Report report, AppUser? user);
    }
}