using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class RegistrationMapper: BaseMapper<DAL.App.DTO.Registration, Domain.App.Registration>
    {
        public RegistrationMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}