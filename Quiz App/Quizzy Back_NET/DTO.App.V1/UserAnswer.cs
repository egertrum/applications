using System;
using Domain.Base;
using DTO.App.V1.Identity;

namespace DTO.App.V1
{
    public class UserAnswer: DomainEntityId
    {
        public Guid? AppUserId { get; set;}
        public AppUser? AppUser { get; set; }
        
        public Guid? QuizId { get; set;}
        public Quiz? Quiz { get; set; }
        
        public Guid? UniqueQuizId { get; set; }
        
        public Guid QuestionAnswerId { get; set;}
        public QuestionAnswer? QuestionAnswer { get; set; }
        
    }
}