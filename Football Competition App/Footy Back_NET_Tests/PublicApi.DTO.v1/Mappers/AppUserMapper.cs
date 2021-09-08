using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class AppUserMapper: BaseMapper<AppUser, BLL.App.DTO.Identity.AppUser>
    {
        public AppUserMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}