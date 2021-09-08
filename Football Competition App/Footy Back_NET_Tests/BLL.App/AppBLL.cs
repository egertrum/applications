using System;
using AutoMapper;
using BLL.App.Mappers;
using BLL.App.Services;
using BLL.Base;
using BLL.Base.Services;
using Contracts.BLL.App;
using Contracts.BLL.App.Services;
using Contracts.BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.Base.Repositories;
using Domain.App;

namespace BLL.App
{
    public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
    {
        protected IMapper Mapper;
        public AppBLL(IAppUnitOfWork uow, IMapper mapper) : base(uow)
        {
            Mapper = mapper;
        }

        public IPersonService Persons =>
            GetService<IPersonService>(() => new PersonService(Uow, Uow.Persons, Mapper));

        public IPersonService AppRoles =>
            GetService<IPersonService>(() => new PersonService(Uow, Uow.Persons, Mapper));

        public ICompetitionService Competitions =>
            GetService<ICompetitionService>(() => new CompetitionService(Uow, Uow.Competitions, Mapper));

        public ICompetitionTeamService CompetitionTeams =>
            GetService<ICompetitionTeamService>(() => new CompetitionTeamService(Uow, Uow.CompetitionTeams, Mapper));
        
        public ICountryService Countries =>
            GetService<ICountryService>(() => new CountryService(Uow, Uow.Countries, Mapper));

        public IEventService Events =>
            GetService<IEventService>(() => new EventService(Uow, Uow.Events, Mapper));

        public IGamePartService GameParts =>
            GetService<IGamePartService>(() => new GamePartService(Uow, Uow.GameParts, Mapper));
        
        public IGameService Games =>
            GetService<IGameService>(() => new GameService(Uow, Uow.Games, Mapper));
        
        public IGameTypeService GameTypes =>
            GetService<IGameTypeService>(() => new GameTypeService(Uow, Uow.GameTypes, Mapper));
        
        public IGamePartTypeService GamePartTypes =>
            GetService<IGamePartTypeService>(() => new GamePartTypeService(Uow, Uow.GamePartTypes, Mapper));

        public IGamePersonService GamePersons =>
            GetService<IGamePersonService>(() => new GamePersonService(Uow, Uow.GamePersons, Mapper));

        public IParticipationService Participations =>
            GetService<IParticipationService>(() => new ParticipationService(Uow, Uow.Participations, Mapper));
        
        public IRegistrationService Registrations =>
            GetService<IRegistrationService>(() => new RegistrationService(Uow, Uow.Registrations, Mapper));

        public IRoleService Roles =>
            GetService<IRoleService>(() => new RoleService(Uow, Uow.Roles, Mapper));

        public ITeamPersonService TeamPersons =>
            GetService<ITeamPersonService>(() => new TeamPersonService(Uow, Uow.TeamPersons, Mapper));
        
        public ITeamService Teams =>
            GetService<ITeamService>(() => new TeamService(Uow, Uow.Teams, Mapper));

        public IAppUserService AppUsers =>
            GetService<IAppUserService>(() => new AppUserService(Uow, Uow.AppUsers, Mapper));
        
        public IReportService Reports =>
            GetService<IReportService>(() => new ReportService(Uow, Uow.Reports, Mapper));
    }
}