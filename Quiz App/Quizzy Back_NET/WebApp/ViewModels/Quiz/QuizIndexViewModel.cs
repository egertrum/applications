using System.Collections.Generic;

namespace WebApp.ViewModels.Quiz
{
    public class QuizIndexViewModel
    {
        public IList<Domain.App.Quiz> Quizzes { get; set; } = default!;

        public string? Error { get; set; }
    }
}