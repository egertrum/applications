using System.Collections.Generic;
using Domain.Base;

namespace WebApp.ViewModels.Question
{
    public class QuestionCreateViewModel
    {
        public Domain.App.Question Question { get; set; } = default!;

        public IList<EQuestionType>? QuestionTypes { get; set; }
    }
}