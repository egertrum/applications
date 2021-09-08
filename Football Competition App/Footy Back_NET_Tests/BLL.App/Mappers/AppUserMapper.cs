using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class AppUserMapper: BaseMapper<BLL.App.DTO.Identity.AppUser, DAL.App.DTO.Identity.AppUser>
    {
        public AppUserMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}