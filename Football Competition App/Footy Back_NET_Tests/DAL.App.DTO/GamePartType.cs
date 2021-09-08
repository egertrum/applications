using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace DAL.App.DTO
{
    public class GamePartType: DomainEntityId
    {
        public Guid NameId { get; set; }
        [MaxLength(128)]public string Name { get; set; } = default!;
        
        public EGamePartType Short { get; set; }

        public ICollection<GamePart>? GameParts { get; set; }
    }
}