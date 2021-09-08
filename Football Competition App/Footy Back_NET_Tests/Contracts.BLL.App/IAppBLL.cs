using System;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base;
using Contracts.BLL.Base.Services;
using BLLAppDTO = BLL.App.DTO;
using DALAppDTO = DAL.App.DTO;
namespace Contracts.BLL.App
{
    public interface IAppBLL : IBaseBLL
    {
        IPersonService Persons { get; }
        ICompetitionService Competitions { get; }
        ICompetitionTeamService CompetitionTeams { get; }
        ICountryService Countries { get; }
        IEventService Events { get; }
        IGamePartService GameParts { get; }
        IGamePartTypeService GamePartTypes { get; }
        IGamePersonService GamePersons { get; }
        IGameService Games { get; }
        IGameTypeService GameTypes { get; }
        IParticipationService Participations { get; }
        IRegistrationService Registrations { get; }
        IRoleService Roles { get; }
        ITeamPersonService TeamPersons { get; }
        ITeamService Teams { get; }
        IReportService Reports { get; }
        
        IAppUserService AppUsers { get; }

        // this stays as BaseService, just for testing
        //IBaseEntityService<BLLAppDTO.Simple, DALAppDTO.Simple> Simples { get; }
    }
}