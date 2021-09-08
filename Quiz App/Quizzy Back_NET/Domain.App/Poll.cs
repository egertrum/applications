using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Poll: DomainEntityId
    {
        [Required]
        [MaxLength(128)] 
        public string Name { get; set; } = default!;
        
        public ICollection<PollQuestion>? PollQuestions { get; set; }
    }
}