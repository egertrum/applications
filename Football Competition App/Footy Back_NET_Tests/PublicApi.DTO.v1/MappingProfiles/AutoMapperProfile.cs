using AutoMapper;

namespace PublicApi.DTO.v1.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Competition, BLL.App.DTO.Competition>().ReverseMap();
            CreateMap<Report, BLL.App.DTO.Report>().ReverseMap();
            CreateMap<AppUser, BLL.App.DTO.Identity.AppUser>().ReverseMap();
            CreateMap<Person, BLL.App.DTO.Person>().ReverseMap();
            CreateMap<Team, BLL.App.DTO.Team>().ReverseMap();
            CreateMap<TeamPerson, BLL.App.DTO.TeamPerson>().ReverseMap();
            CreateMap<Country, BLL.App.DTO.Country>().ReverseMap();
            CreateMap<Role, BLL.App.DTO.Role>().ReverseMap();
            CreateMap<CompetitionTeam, BLL.App.DTO.CompetitionTeam>().ReverseMap();
            CreateMap<Game, BLL.App.DTO.Game>().ReverseMap();
            CreateMap<GamePartType, BLL.App.DTO.GamePartType>().ReverseMap();
            CreateMap<GamePart, BLL.App.DTO.GamePart>().ReverseMap();
            CreateMap<GameType, BLL.App.DTO.GameType>().ReverseMap();
            CreateMap<GameLength, BLL.App.DTO.GameLength>().ReverseMap();
            CreateMap<Registration, BLL.App.DTO.Registration>().ReverseMap();
        }

    }
}