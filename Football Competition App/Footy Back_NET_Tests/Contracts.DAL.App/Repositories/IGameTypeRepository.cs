using System;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IGameTypeRepository: IBaseRepository<DALAppDTO.GameType>, IGameTypeRepositoryCustom<DALAppDTO.GameType>
    {
    }

    public interface IGameTypeRepositoryCustom<TEntity>
    {
    }
}