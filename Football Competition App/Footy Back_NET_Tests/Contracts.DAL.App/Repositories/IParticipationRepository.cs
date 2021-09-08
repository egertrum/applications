using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IParticipationRepository: IBaseRepository<DALAppDTO.Participation>, IParticipationRepositoryCustom<DALAppDTO.Participation>
    {
    }

    public interface IParticipationRepositoryCustom<TEntity>
    {
    }
}