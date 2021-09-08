using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO.Identity;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App.Repositories;
using AppUser = Domain.App.Identity.AppUser;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;

namespace Contracts.BLL.App.Services
{
    public interface ICompetitionService : IBaseEntityService<BLLAppDTO.Competition, DALAppDTO.Competition>,
        ICompetitionRepositoryCustom<BLLAppDTO.Competition>
    {
        public Task MakeRegistration(Guid userId, Guid competitionId, bool noTracking = true);
    }
}