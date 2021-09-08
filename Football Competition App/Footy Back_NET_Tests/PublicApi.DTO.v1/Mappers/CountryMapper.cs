using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class CountryMapper : BaseMapper<Country, BLL.App.DTO.Country>
    {
        public CountryMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}