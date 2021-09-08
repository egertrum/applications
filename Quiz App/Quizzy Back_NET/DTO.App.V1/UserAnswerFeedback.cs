using System.Collections.Generic;

namespace DTO.App.V1
{
    public class UserAnswerFeedback
    {
        public IEnumerable<DTO.App.V1.UserAnswer> UserAnswers { get; set; } = default!;

        public int CorrectAnswers { get; set; }
        
        public int MaxPoints { get; set; }
    }
}