using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class Country: DomainEntityId
    {
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        public Guid NameId { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.Views.Shared.Basic))] 
        [MaxLength(256, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        public string Name { get; set; } = default!;
        
    }
}