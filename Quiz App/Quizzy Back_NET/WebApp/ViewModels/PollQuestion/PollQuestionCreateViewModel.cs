using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.PollQuestion
{
    public class PollQuestionCreateViewModel
    {
        public Domain.App.PollQuestion PollQuestion { get; set; } = default!;

        public SelectList? QuestionsSelectList { get; set; }
        
        public SelectList? PollsSelectList { get; set; }
    }
}