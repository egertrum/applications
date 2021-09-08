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
    public class EventService: BaseEntityService<IAppUnitOfWork, IEventRepository, BLLAppDTO.Event, DALAppDTO.Event>, IEventService
    {
        public EventService(IAppUnitOfWork serviceUow, IEventRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new EventMapper(mapper))
        {
        }
    }

}