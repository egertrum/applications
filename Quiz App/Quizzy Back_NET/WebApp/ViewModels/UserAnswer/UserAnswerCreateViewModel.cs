using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.UserAnswer
{
    public class UserAnswerCreateViewModel
    {
        public Domain.App.UserAnswer UserAnswer { get; set; } = default!;
        public Guid QuizId { get; set; }
        
        public Guid QuizUniqueId { get; set; }

        public string Question { get; set; } = default!;
        
        public SelectList? QuestionAnswersSelectList { get; set; }
        
        public int NextNumber { get; set; }

        public bool LastQuestion { get; set; }
        
        public bool? Poll { get; set; }
    }
}