using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class AppUserMapper: BaseMapper<DAL.App.DTO.Identity.AppUser, Domain.App.Identity.AppUser>
    {
        public AppUserMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}