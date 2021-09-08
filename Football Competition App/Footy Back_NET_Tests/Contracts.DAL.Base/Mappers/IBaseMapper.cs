namespace Contracts.DAL.Base.Mappers
{
    public interface IBaseMapper<TLeftObject, TRightObject>
    {
        TLeftObject? Map(TRightObject? inObject);
        TRightObject? Map(TLeftObject? inObject);
    }
}