using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class RegistrationMapper: BaseMapper<BLL.App.DTO.Registration, DAL.App.DTO.Registration>
    {
        public RegistrationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}