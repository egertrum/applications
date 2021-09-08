using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class Registration: DomainEntityId
    {
        [Display(Name = nameof(CompetitionId), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public Guid CompetitionId { get; set;}
        [Display(Name = nameof(CompetitionId), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public Competition? Competition { get; set; }
        
        [Display(Name = nameof(TeamId), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public Guid? TeamId { get; set;}
        [Display(Name = nameof(TeamId), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public Team? Team { get; set; }

        [Display(Name = nameof(AppUserId), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public Guid AppUserId { get; set; }
        [Display(Name = nameof(AppUserId), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public AppUser? AppUser { get; set; }

        [DataType(DataType.Date)] 
        [Display(Name = nameof(Date), ResourceType = typeof(Base.Resources.DTO.v1.Registration))]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Display(Name = nameof(Comment), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        [MaxLength(Int32.MaxValue)]
        public string? Comment { get; set; }
    }
}