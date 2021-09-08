using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using Domain.Base;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface IGamePartTypeRepository: IBaseRepository<DALAppDTO.GamePartType>, IGamePartTypeRepositoryCustom<DALAppDTO.GamePartType>
    {
    }

    public interface IGamePartTypeRepositoryCustom<TEntity>
    {
        public Task<Guid> FindIdByShort(EGamePartType shortName);
    }
}