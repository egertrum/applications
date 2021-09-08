using System.Collections.Generic;
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
    public class CountryService: BaseEntityService<IAppUnitOfWork, ICountryRepository, BLLAppDTO.Country, DALAppDTO.Country>, ICountryService
    {
        public CountryService(IAppUnitOfWork serviceUow, ICountryRepository serviceRepository, IMapper mapper) : base(serviceUow, serviceRepository, new CountryMapper(mapper))
        {
        }

        public Task<IEnumerable<BLLAppDTO.Country>> GetAllWithCountsAsync(bool noTracking = true)
        {
            throw new System.NotImplementedException();
        }
    }

}