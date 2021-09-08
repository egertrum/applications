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
    public class ReportService: BaseEntityService<IAppUnitOfWork, IReportRepository, BLLAppDTO.Report, DALAppDTO.Report>, IReportService
    {
        public ReportService(IAppUnitOfWork serviceUow, IReportRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new ReportMapper(mapper))
        {
        }

        public BLLAppDTO.Report AddSubmitter(BLLAppDTO.Report report, Domain.App.Identity.AppUser? user)
        {
            report.AppUserId = user?.Id;
            report.Submitter = user != null ? user.FullName : "Guest";
            report.Date = DateTime.Now;
            return report;
        }
    }
}