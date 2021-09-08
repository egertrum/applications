using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class AppRoleMapper: BaseMapper<DAL.App.DTO.Identity.AppRole, Domain.App.Identity.AppRole>
    {
        public AppRoleMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}