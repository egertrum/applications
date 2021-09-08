using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DTO.App.V1
{
    public class Poll: DomainEntityId
    {
        [Required]
        [MaxLength(128)] 
        public string Name { get; set; } = default!;
        
    }
}