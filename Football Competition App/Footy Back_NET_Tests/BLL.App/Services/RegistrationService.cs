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
    public class RegistrationService: BaseEntityService<IAppUnitOfWork, IRegistrationRepository, BLLAppDTO.Registration, DALAppDTO.Registration>, IRegistrationService
    {
        public RegistrationService(IAppUnitOfWork serviceUow, IRegistrationRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new RegistrationMapper(mapper))
        {
        }

        public Task<bool> CompetitionRegisteredToUser(Guid userId, Guid compId, bool noTracking = true)
        {
            return ServiceRepository.CompetitionRegisteredToUser(userId, compId, noTracking);
        }

        public async Task<BLLAppDTO.Registration> GetByCompetitionId(Guid compId, bool noTracking = true)
        {
            return Mapper.Map(await ServiceRepository.GetByCompetitionId(compId, noTracking))!;
        }
    }

}