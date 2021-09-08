using AutoMapper;
using DTO.App.V1.Identity;

namespace DTO.App.V1.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Domain.App.Poll, DTO.App.V1.Poll>().ReverseMap();
            CreateMap<Domain.App.PollQuestion, DTO.App.V1.PollQuestion>().ReverseMap();
            CreateMap<Domain.App.Identity.AppUser, DTO.App.V1.Identity.AppUser>().ReverseMap();
            CreateMap<Domain.App.Question, DTO.App.V1.Question>().ReverseMap();
            CreateMap<Domain.App.QuestionAnswer, DTO.App.V1.QuestionAnswer>().ReverseMap();
            CreateMap<Domain.App.Quiz, DTO.App.V1.Quiz>().ReverseMap();
            CreateMap<Domain.App.QuizQuestion, DTO.App.V1.QuizQuestion>().ReverseMap();
            CreateMap<Domain.App.Score, DTO.App.V1.Score>().ReverseMap();
            CreateMap<Domain.App.UserAnswer, DTO.App.V1.UserAnswer>().ReverseMap();
        }

    }
}