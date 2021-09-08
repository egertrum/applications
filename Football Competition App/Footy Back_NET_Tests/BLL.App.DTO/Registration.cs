using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Registration: DomainEntityId
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