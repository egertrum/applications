using AutoMapper;

namespace DTO.App.V1.Mappers
{
    public class BaseMapper<TLeftEntity, TRightEntity>
    {
        protected IMapper Mapper;
        
        public BaseMapper(IMapper mapper)
        {
            Mapper = mapper;
        }
        public TLeftEntity? Map(TRightEntity? inObject)
        {
            return Mapper.Map<TLeftEntity>(inObject);
        }

        public TRightEntity? Map(TLeftEntity? inObject)
        {
            return Mapper.Map<TRightEntity>(inObject);
        }
    }
}