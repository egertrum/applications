using System.Collections.Generic;

namespace WebApp.ViewModels.Question
{
    public class QuestionIndexViewModel
    {
        public IList<Domain.App.Question> Questions { get; set; } = default!;

        public string? Error { get; set; }
    }
}