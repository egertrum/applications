using AutoMapper;
using BLL.App.DTO.Identity;

namespace BLL.App.DTO.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Competition, DAL.App.DTO.Competition>().ReverseMap();
            CreateMap<CompetitionTeam, DAL.App.DTO.CompetitionTeam>().ReverseMap();
            CreateMap<Country, DAL.App.DTO.Country>().ReverseMap();
            CreateMap<Event, DAL.App.DTO.Event>().ReverseMap();
            CreateMap<GameType, DAL.App.DTO.GameType>().ReverseMap();
            CreateMap<Game, DAL.App.DTO.Game>().ReverseMap();
            CreateMap<GamePartType, DAL.App.DTO.GamePartType>().ReverseMap();
            CreateMap<GamePart, DAL.App.DTO.GamePart>().ReverseMap();
            CreateMap<GamePerson, DAL.App.DTO.GamePerson>().ReverseMap();
            CreateMap<Participation, DAL.App.DTO.Participation>().ReverseMap();
            CreateMap<Person, DAL.App.DTO.Person>().ReverseMap();
            CreateMap<Registration, DAL.App.DTO.Registration>().ReverseMap();
            CreateMap<Role, DAL.App.DTO.Role>().ReverseMap();
            CreateMap<TeamPerson, DAL.App.DTO.TeamPerson>().ReverseMap();
            CreateMap<Team, DAL.App.DTO.Team>().ReverseMap();
            CreateMap<Report, DAL.App.DTO.Report>().ReverseMap();
            CreateMap<AppUser, DAL.App.DTO.Identity.AppUser>().ReverseMap();
            CreateMap<AppRole, DAL.App.DTO.Identity.AppRole>().ReverseMap();
        }

    }
}