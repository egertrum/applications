using Contracts.DAL.App.Repositories;
using Contracts.DAL.Base;
using Contracts.DAL.Base.Repositories;
using Domain.App;
using Domain.App.Identity;
using AppUser = DAL.App.DTO.Identity.AppUser;

namespace Contracts.DAL.App
{
    public interface IAppUnitOfWork : IBaseUnitOfWork
    {
        IPersonRepository Persons { get; }
        /* WE DONT HAVE ANY CUSTOM METHODS YET
        IContactRepository Contacts { get; }
        IContactTypeRepository ContactTypes { get; }
        IPersonPictureRepository PersonPictures { get; }
        */
        
        ICompetitionRepository Competitions { get; }
        ICompetitionTeamRepository CompetitionTeams { get; }
        ICountryRepository Countries { get; }
        
        IEventRepository Events { get; }
        IGamePartRepository GameParts { get; }
        IGamePartTypeRepository GamePartTypes { get; }
        
        IGamePersonRepository GamePersons { get; }
        IGameRepository Games { get; }
        
        IGameTypeRepository GameTypes { get; }
        IParticipationRepository Participations { get; }
        
        IRegistrationRepository Registrations { get; }
        IRoleRepository Roles { get; }
        ITeamPersonRepository TeamPersons { get; }
        ITeamRepository Teams { get; }
        
        IReportRepository Reports { get; }
        
        IAppUserRepository<AppUser> AppUsers { get; }
        
    }
}