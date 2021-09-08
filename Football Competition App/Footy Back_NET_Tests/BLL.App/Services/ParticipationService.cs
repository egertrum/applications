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
    public class ParticipationService: BaseEntityService<IAppUnitOfWork, IParticipationRepository, BLLAppDTO.Participation, DALAppDTO.Participation>, IParticipationService
    {
        public ParticipationService(IAppUnitOfWork serviceUow, IParticipationRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new ParticipationMapper(mapper))
        {
        }
    }

}