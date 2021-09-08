using System;
using System.Collections.Generic;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
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