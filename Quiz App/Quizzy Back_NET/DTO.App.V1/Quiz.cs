using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DTO.App.V1
{
    public class Quiz: DomainEntityId
    {
        [Required]
        [MaxLength(128)] 
        public string Name { get; set; } = default!;

        public int? PassingProc { get; set; }
        
        public double? Average { get; set; }

        public int MaxPoints { get; set; }
        
    }
}