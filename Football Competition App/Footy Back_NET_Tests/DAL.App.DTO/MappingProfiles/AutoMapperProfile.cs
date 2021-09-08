using AutoMapper;

namespace DAL.App.DTO.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.App.DTO.Competition, Domain.App.Competition>().ReverseMap();
            CreateMap<DAL.App.DTO.CompetitionTeam, Domain.App.CompetitionTeam>().ReverseMap();
            CreateMap<DAL.App.DTO.Country, Domain.App.Country>().ReverseMap();
            CreateMap<DAL.App.DTO.Event, Domain.App.Event>().ReverseMap();
            CreateMap<DAL.App.DTO.Game, Domain.App.Game>().ReverseMap();
            CreateMap<DAL.App.DTO.GameType, Domain.App.GameType>().ReverseMap();
            CreateMap<DAL.App.DTO.GamePartType, Domain.App.GamePartType>().ReverseMap();
            CreateMap<DAL.App.DTO.GamePart, Domain.App.GamePart>().ReverseMap();
            CreateMap<DAL.App.DTO.GamePerson, Domain.App.GamePerson>().ReverseMap();
            CreateMap<DAL.App.DTO.Participation, Domain.App.Participation>().ReverseMap();
            CreateMap<DAL.App.DTO.Person, Domain.App.Person>().ReverseMap();
            CreateMap<DAL.App.DTO.Registration, Domain.App.Registration>().ReverseMap();
            CreateMap<DAL.App.DTO.Role, Domain.App.Role>().ReverseMap();
            CreateMap<DAL.App.DTO.TeamPerson, Domain.App.TeamPerson>().ReverseMap();
            CreateMap<DAL.App.DTO.Team, Domain.App.Team>().ReverseMap();
            CreateMap<DAL.App.DTO.Report, Domain.App.Report>().ReverseMap();
            CreateMap<DAL.App.DTO.Identity.AppUser, Domain.App.Identity.AppUser>().ReverseMap();
            CreateMap<DAL.App.DTO.Identity.AppRole, Domain.App.Identity.AppRole>().ReverseMap();
        }

    }
}