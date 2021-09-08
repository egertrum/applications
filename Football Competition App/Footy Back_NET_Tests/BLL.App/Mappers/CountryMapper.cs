using AutoMapper;
using Contracts.BLL.Base.Mappers;

namespace BLL.App.Mappers
{
    public class CountryMapper: BaseMapper<BLL.App.DTO.Country, DAL.App.DTO.Country>
    {
        public CountryMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}