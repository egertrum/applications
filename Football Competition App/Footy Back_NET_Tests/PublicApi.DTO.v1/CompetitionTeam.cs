using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class CompetitionTeam: DomainEntityId
    {

        [DataType(DataType.Date)] 
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Since), ResourceType = typeof(Base.Resources.DTO.v1.CompetitionTeam))] 
        public DateTime Since { get; set; } = DateTime.Now;
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(TeamId), ResourceType = typeof(Base.Resources.DTO.v1.CompetitionTeam))] 
        public Guid TeamId { get; set;}
        public Team? Team { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(CompetitionId), ResourceType = typeof(Base.Resources.DTO.v1.CompetitionTeam))] 
        public Guid CompetitionId { get; set;}
        public Competition? Competition { get; set; }
    }
}