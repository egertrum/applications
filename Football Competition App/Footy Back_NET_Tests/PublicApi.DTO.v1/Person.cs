using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using BLL.App.DTO;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class Person: DomainEntityId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(CountryId), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        public Guid CountryId { get; set;}
        
        [Display(Name = nameof(CountryId), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        public Country? Country { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(FirstName), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        public string FirstName { get; set; } = default!;
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(LastName), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        [MaxLength(128, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        public string LastName { get; set; } = default!;
        
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        public string Name => FirstName + " " + LastName;

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(IdentificationCode), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        public string IdentificationCode { get; set; } = default!;

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "BirthDate", ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        public DateTime BirthDate { get; set; } = default!;
        
        [Display(Name = nameof(Gender), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        [MaxLength(16, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")]  
        public string? Gender { get; set; } //controversial
        
        [Display(Name = nameof(PersonTeams), ResourceType = typeof(Base.Resources.DTO.v1.Person))] 
        public IDictionary<string, int>? PersonTeams { get; set; }
        
        [Display(Name = nameof(AppUserId), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        public Guid AppUserId { get; set; }
    }
}