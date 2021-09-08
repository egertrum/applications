using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IGamePersonRepository: IBaseRepository<DALAppDTO.GamePerson>, IGamePersonRepositoryCustom<DALAppDTO.GamePerson>
    {
    }

    public interface IGamePersonRepositoryCustom<TEntity>
    {
        public int GetGamesCountForPlayer(Guid personId);
    }
}