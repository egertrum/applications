using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class PollQuestion: DomainEntityId
    {
        [Required]
        public Guid PollId { get; set;}
        public Poll? Poll { get; set; }

        [Required]
        public Guid QuestionId { get; set;}
        public Question? Question { get; set; }

        public int Number { get; set; }
    }
}