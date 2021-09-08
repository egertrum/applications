using System.Collections.Generic;
using Domain.App;

namespace WebApp.ViewModels.UserAnswer
{
    public class UserAnswerFeedBackViewModel
    {
        public IList<Domain.App.UserAnswer> UserAnswers { get; set; } = default!;

        public int CorrectAnswers { get; set; }
        
        public int MaxPoints { get; set; }
    }
}