using System;
using System.ComponentModel.DataAnnotations;
using Base.Resources;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class TeamPerson: DomainEntityId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(TeamId), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))]
        public Guid TeamId { get; set;}

        [Display(Name = nameof(Team), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))]
        public Team? Team { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(PersonId), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))] 
        public Guid PersonId { get; set;}
        
        [Display(Name = nameof(Person), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))]
        public Person? Person { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(RoleId), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))] 
        public Guid RoleId { get; set;}
        
        [Display(Name = nameof(Role), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))]
        public Role? Role { get; set; }
        
        [DataType(DataType.Date)] 
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessage_Required")]
        [Display(Name = nameof(Since), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))] 
        public DateTime Since { get; set; }
        
        [DataType(DataType.Date)] 
        [Display(Name = nameof(Until), ResourceType = typeof(Base.Resources.DTO.v1.TeamPerson))] 
        public DateTime? Until { get; set; }
    }
}