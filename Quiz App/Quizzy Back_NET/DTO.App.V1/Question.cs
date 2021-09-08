using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DTO.App.V1
{
    public class Question: DomainEntityId
    {
        [Required]
        [MaxLength(4064)] 
        public string Value { get; set; } = default!;

        [Required] public string QuestionType { get; set; } = default!;

    }
}