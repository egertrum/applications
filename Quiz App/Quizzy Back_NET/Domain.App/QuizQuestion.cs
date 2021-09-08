using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class QuizQuestion: DomainEntityId
    {
        [Required]
        public Guid QuizId { get; set;}
        public Quiz? Quiz { get; set; }

        [Required]
        public Guid QuestionId { get; set;}
        public Question? Question { get; set; }

        public int Number { get; set; }
    }
}