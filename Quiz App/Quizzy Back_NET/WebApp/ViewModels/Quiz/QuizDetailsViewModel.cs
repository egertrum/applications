using System.Collections.Generic;

namespace WebApp.ViewModels.Quiz
{
    public class QuizDetailsViewModel
    {
        public Domain.App.Quiz Quiz { get; set; } = default!;
        
        public IList<Domain.App.QuizQuestion> QuizQuestions { get; set; } = default!;
    }
}