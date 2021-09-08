using AutoMapper;

namespace PublicApi.DTO.v1.Mappers
{
    public class PersonMapper: BaseMapper<Person, BLL.App.DTO.Person>
    {
        public PersonMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}