using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base.Repositories;
using DAL.App.DTO.Identity;
using DAL.App.EF.Repositories;
using DAL.Base.EF;
using DAL.Base.EF.Repositories;
using Domain.App;

namespace DAL.App.EF
{
    public class AppUnitOfWork : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
    {
        protected IMapper Mapper;
        public AppUnitOfWork(AppDbContext uowDbContext, IMapper mapper) : base(uowDbContext)
        {
            Mapper = mapper;
        }
        
        public IPersonRepository Persons => 
            GetRepository(() => new PersonRepository(UowDbContext, Mapper));

        public ICompetitionRepository Competitions =>
            GetRepository(() => new CompetitionRepository(UowDbContext, Mapper));

        public ICompetitionTeamRepository CompetitionTeams =>
            GetRepository(() => new CompetitionTeamRepository(UowDbContext, Mapper));

        public ICountryRepository Countries =>
            GetRepository(() => new CountryRepository(UowDbContext, Mapper));
        
        public IEventRepository Events => 
            GetRepository(() => new EventRepository(UowDbContext, Mapper));

        public IGameRepository Games =>
            GetRepository(() => new GameRepository(UowDbContext, Mapper));
        
        public IGameTypeRepository GameTypes =>
            GetRepository(() => new GameTypeRepository(UowDbContext, Mapper));

        public IGamePartRepository GameParts =>
            GetRepository(() => new GamePartRepository(UowDbContext, Mapper));

        public IGamePartTypeRepository GamePartTypes =>
            GetRepository(() => new GamePartTypeRepository(UowDbContext, Mapper));
        public IGamePersonRepository GamePersons => 
            GetRepository(() => new GamePersonRepository(UowDbContext, Mapper));

        public IParticipationRepository Participations =>
            GetRepository(() => new ParticipationRepository(UowDbContext, Mapper));

        public IRegistrationRepository Registrations =>
            GetRepository(() => new RegistrationRepository(UowDbContext, Mapper));

        public IRoleRepository Roles =>
            GetRepository(() => new RoleRepository(UowDbContext, Mapper));
        public ITeamRepository Teams => 
            GetRepository(() => new TeamRepository(UowDbContext, Mapper));

        public IAppUserRepository<AppUser> AppUsers => 
            GetRepository(() => new AppUserRepository(UowDbContext, Mapper));

        public ITeamPersonRepository TeamPersons =>
            GetRepository(() => new TeamPersonRepository(UowDbContext, Mapper));
        
        public IReportRepository Reports =>
            GetRepository(() => new ReportRepository(UowDbContext, Mapper));
    }
}