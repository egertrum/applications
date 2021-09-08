using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.QuestionAnswer
{
    public class QuestionAnswerCreateViewModel
    {
        public Domain.App.QuestionAnswer QuestionAnswer { get; set; } = default!;

        public SelectList? QuestionsSelectList { get; set; }
        
        public bool? PollQuestion { get; set; }
    }
}