using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.App.EF.Mappers
{
    public class CountryMapper: BaseMapper<DAL.App.DTO.Country, Domain.App.Country>
    {
        public CountryMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}