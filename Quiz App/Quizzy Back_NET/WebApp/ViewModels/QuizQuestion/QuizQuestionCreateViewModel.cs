using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels.QuizQuestion
{
    public class QuizQuestionCreateViewModel
    {
        public Domain.App.QuizQuestion QuizQuestion { get; set; } = default!;

        public SelectList? QuestionsSelectList { get; set; }
        
        public SelectList? QuizzesSelectList { get; set; }
    }
}