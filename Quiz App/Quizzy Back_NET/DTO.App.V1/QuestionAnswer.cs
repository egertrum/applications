using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DTO.App.V1
{
    public class QuestionAnswer: DomainEntityId
    {
        [Required]
        public Guid QuestionId { get; set;}
        public Question? Question { get; set; }

        [MaxLength(4064)]
        public string Value { get; set; } = default!;

        public bool True { get; set; }
    }
}