using System;
using System.Collections.Generic;

namespace WebApp.ViewModels.QuestionAnswer
{
    public class QuestionAnswerIndexViewModel
    {
        public IList<Domain.App.QuestionAnswer> QuestionAnswers { get; set; } = default!;
        
        public Guid? QuestionId { get; set; }

        public string Title { get; set; } = default!;
    }
}