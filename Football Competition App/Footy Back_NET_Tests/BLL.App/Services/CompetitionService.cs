using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.Identity;
using BLL.App.Mappers;
using BLL.Base.Services;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;


namespace BLL.App.Services
{
    public class CompetitionService: BaseEntityService<IAppUnitOfWork, ICompetitionRepository, BLLAppDTO.Competition, DALAppDTO.Competition>, ICompetitionService
    {
        public CompetitionService(IAppUnitOfWork serviceUow, ICompetitionRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new CompetitionMapper(mapper))
        {
        }

        public async Task MakeRegistration(Guid userId, Guid competitionId, bool noTracking = true)
        {
            //Task<BLLAppDTO.Registration>
            var reg = new DAL.App.DTO.Registration
            {
                CompetitionId = competitionId,
                AppUserId = userId,
            };
            ServiceUow.Registrations.Add(reg);
            await ServiceUow.SaveChangesAsync();
        }

        public async Task<IEnumerable<BLLAppDTO.Competition>> GetAllAsyncWithSearch(BLLAppDTO.Competition? searchCompetition)
        {
            return (await ServiceRepository.GetAllAsyncWithSearch(Mapper.Map(searchCompetition)!)).Select(
                x => Mapper.Map(x))!;
        }
    }

}