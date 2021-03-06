using System;
using System.Collections.Generic;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Event: DomainEntityId
    {
        public Guid GameId { get; set; } 
        public Game? Game { get; set; }
        
        public Guid GamePartId { get; set; }
        public GamePart? GamePart { get; set; }
        
        public DateTime? From { get; set; }
        
        public DateTime? Until { get; set; }
        
        public string Description { get; set; } = default!;
        
        
        public ICollection<Participation>? Participators { get; set; }
    }
}