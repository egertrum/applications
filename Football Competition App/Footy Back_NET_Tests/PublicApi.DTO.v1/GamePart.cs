using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class GamePart: DomainEntityId
    {
        [Display(Name = nameof(GameId), ResourceType = typeof(Base.Resources.DTO.v1.GamePart))]
        public Guid GameId { get; set; }
        
        [Display(Name = nameof(Game), ResourceType = typeof(Base.Resources.DTO.v1.GamePart))]
        public Game? Game { get; set; }
        
        [Display(Name = nameof(GamePartTypeId), ResourceType = typeof(Base.Resources.DTO.v1.GamePart))]
        public Guid GamePartTypeId { get; set; }
        [Display(Name = nameof(GamePartType), ResourceType = typeof(Base.Resources.DTO.v1.GamePart))]
        public GamePartType? GamePartType { get; set; }
        
        public string Name => GamePartType?.Name ?? "";

        [Display(Name = nameof(Length), ResourceType = typeof(Base.Resources.DTO.v1.GamePart))]
        public int Length { get; set; }

        public int? AdditionalTime { get; set; }
    }
}