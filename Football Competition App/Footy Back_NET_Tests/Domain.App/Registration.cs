using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using Domain.App.Identity;
using Domain.Base;

namespace Domain.App
{
    public class Registration: DomainEntityId, IDomainAppUser<AppUser>, IDomainAppUserId<Guid>
    {
        public Guid CompetitionId { get; set;}
        public Competition? Competition { get; set; }
        
        public Guid? TeamId { get; set;}
        public Team? Team { get; set; }

        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        [DataType(DataType.Date)] 
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [MaxLength(Int32.MaxValue)]public string? Comment { get; set; }
    }
}