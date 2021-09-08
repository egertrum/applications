using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class Team: DomainEntityId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(CountryId), ResourceType = typeof(Base.Resources.DTO.v1.Team))]
        public Guid CountryId { get; set;}
        
        [Display(Name = nameof(Country), ResourceType = typeof(Base.Resources.DTO.v1.Team))] 
        public Country? Country { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(AppUserId), ResourceType = typeof(Base.Resources.DTO.v1.Team))]
        public Guid AppUserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.Views.Shared.Basic))] 
        [MaxLength(256, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        public string Name { get; set; } = default!;
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(PlayersAmount), ResourceType = typeof(Base.Resources.DTO.v1.Team))]
        public int PlayersAmount { get; set; }
    }
}