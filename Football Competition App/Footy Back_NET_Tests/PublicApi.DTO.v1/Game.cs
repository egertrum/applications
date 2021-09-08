using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class Game: DomainEntityId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(HomeId), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public Guid HomeId { get; set; }
        
        [Display(Name = nameof(HomeId), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public Team? Home { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(AwayId), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public Guid AwayId { get; set; }
        
        [Display(Name = nameof(AwayId), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public Team? Away { get; set; }
        
        public Guid CompetitionId { get; set; }
        public Competition? Competition { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(GameTypeId), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public Guid GameTypeId { get; set; }
        
        [Display(Name = nameof(GameTypeId), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public GameType? GameType { get; set; }

        public string Name => Home?.Name + " vs " + Away?.Name;
        
        [Display(Name = nameof(KickOffTime), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public DateTime? KickOffTime { get; set; }

        [Display(Name = nameof(HomeScore), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public int? HomeScore { get; set; }
        
        [Display(Name = nameof(AwayScore), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public int? AwayScore { get; set; }
        
        public string Score => HomeScore + " - " + AwayScore;
        
        [Display(Name = nameof(Comment), ResourceType = typeof(Base.Resources.DTO.v1.Game))] 
        public string? Comment { get; set; }

        public Guid? IdOfGameWinner { get; set; }
    }
}