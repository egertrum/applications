using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO.Identity;
using BLL.App.Mappers;
using Contracts.BLL.App.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;


namespace BLL.App.Services
{
    public class AppUserService: IAppUserService
    {
        private IMapper _mapper;
        private IAppUserRepository<DAL.App.DTO.Identity.AppUser> _repo;
        public AppUserService(IAppUnitOfWork serviceUow, IAppUserRepository<DAL.App.DTO.Identity.AppUser> serviceRepository, IMapper mapper)
        {
            _mapper = mapper;
            _repo = serviceRepository;
        }

        public async Task<AppUser> FirstOrDefaultAsync(Guid id, bool noTracking = true)
        {
            var mapper = new AppUserMapper(_mapper);
            return mapper.Map(await _repo.FirstOrDefaultAsync(id, noTracking))!;
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync(bool noTracking = true)
        {
            var mapper = new AppUserMapper(_mapper);
            return (await _repo.GetAllAsync()).Select(x => mapper.Map(x))!;
        }
    }

}