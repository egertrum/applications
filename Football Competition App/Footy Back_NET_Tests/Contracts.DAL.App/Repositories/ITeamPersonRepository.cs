using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.DAL.App.Repositories
{
    public interface ITeamPersonRepository: IBaseRepository<DALAppDTO.TeamPerson>, ITeamPersonRepositoryCustom<DALAppDTO.TeamPerson>
    {
    }

    public interface ITeamPersonRepositoryCustom<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllWithTeamId(Guid teamId);
        
        public Task<bool> IsPossibleToAdd(DALAppDTO.TeamPerson teamPerson);
    }
    
}