using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Question: DomainEntityId
    {
        [Required]
        [MaxLength(4064)] 
        public string Value { get; set; } = default!;

        [Required]
        public EQuestionType QuestionType { get; set; }
        
        public ICollection<QuizQuestion>? QuizQuestions { get; set; }
        
        public ICollection<PollQuestion>? PollQuestions { get; set; }
        
        public ICollection<QuestionAnswer>? QuestionAnswers { get; set; }
    }
}