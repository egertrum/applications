using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class RegistrationMapper: BaseMapper<Registration, BLL.App.DTO.Registration>
    {
        public RegistrationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}