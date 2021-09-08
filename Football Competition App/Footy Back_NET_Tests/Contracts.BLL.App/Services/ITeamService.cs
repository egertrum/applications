using System;
using System.Threading.Tasks;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ITeamService: IBaseEntityService<BLLAppDTO.Team, DALAppDTO.Team>, ITeamRepositoryCustom<BLLAppDTO.Team>
    {
        public Task RemoveTeam(Guid teamId, bool noTracking = true);
    }
}