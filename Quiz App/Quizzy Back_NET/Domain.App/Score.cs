using System;
using Domain.Base;

namespace Domain.App
{
    public class Score: DomainEntityId
    {
        public Guid QuizId { get; set; }
        public Quiz? Quiz { get; set; }
        
        public int Amount { get; set; }

        public bool Passed { get; set; }
    }
}