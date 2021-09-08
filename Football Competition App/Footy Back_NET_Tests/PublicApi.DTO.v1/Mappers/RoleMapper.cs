using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class RoleMapper: BaseMapper<Role, BLL.App.DTO.Role>
    {
        public RoleMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}