using AutoMapper;
using DTO.App.V1.Identity;

namespace DTO.App.V1.Mappers
{
    public class AppUserMapper: BaseMapper<Domain.App.Identity.AppUser, DTO.App.V1.Identity.AppUser>
    {
        public AppUserMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}