using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using BLL.App.DTO;
using Domain.Base;
using Domain.Base.Identity;

namespace PublicApi.DTO.v1
{
    public class Competition: DomainEntityId
    {

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(CountryId), ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public Guid CountryId { get; set;}
        
        [Display(Name = nameof(Country), ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public Country? Country { get; set; }

        [MaxLength(256, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "Name", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public string Name { get; set; } = default!;
        
        [MaxLength(64, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "Organiser", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public string Organiser { get; set; } = default!;
        
        [DataType(DataType.Date)] 
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = "StartDate", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public DateTime StartDate { get; set; }
            
        [DataType(DataType.Date)] 
        [Display(Name = "EndDate", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public DateTime? EndDate { get; set; }
        
        [MaxLength(4064, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        [Display(Name = "Comment", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public string? Comment { get; set; }
    }
    
    public class SearchCompetition
    {
        [Display(Name = nameof(CountryId), ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public Guid? CountryId { get; set;}

        [MaxLength(256, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_StringLengthMax")] 
        [Display(Name = "Name", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public string? Name { get; set; }

        [DataType(DataType.Date)] 
        [Display(Name = "StartDate", ResourceType = typeof(Base.Resources.DTO.v1.Competition))] 
        public DateTime? StartDate { get; set; }
    }
}