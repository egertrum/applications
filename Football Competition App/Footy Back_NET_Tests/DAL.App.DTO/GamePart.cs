using System;
using System.Collections.Generic;
using Domain.Base;

namespace DAL.App.DTO
{
    public class GamePart: DomainEntityId
    {
        public Guid GameId { get; set; }
        public Game? Game { get; set; }
        
        public Guid GamePartTypeId { get; set; }
        public GamePartType? GamePartType { get; set; }
        
        public string Name => GamePartType?.Name ?? "";

        public int Length { get; set; }

        public int? AdditionalTime { get; set; }
        
        public ICollection<Event>? Events { get; set; }
    }
}