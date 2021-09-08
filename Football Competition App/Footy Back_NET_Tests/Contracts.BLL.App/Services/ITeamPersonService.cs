using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ITeamPersonService: IBaseEntityService<BLLAppDTO.TeamPerson, DALAppDTO.TeamPerson>, ITeamPersonRepositoryCustom<BLLAppDTO.TeamPerson>
    {
        public Task<BLLAppDTO.TeamPerson?> AddIfPossible(BLLAppDTO.TeamPerson teamPerson);

        public Task<bool> TeamAndPersonBelongToUser(BLLAppDTO.TeamPerson teamPerson, Guid userId);
    }
}