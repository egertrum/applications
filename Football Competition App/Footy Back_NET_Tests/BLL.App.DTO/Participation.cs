using System;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Participation: DomainEntityId
    {
        public Guid PersonId { get; set; }
        public Person? Person { get; set; }
        
        public Guid? TeamId { get; set; }
        public Team? Team { get; set; }
        
        public Guid EventId { get; set; }
        public Event? Event { get; set; }
        
        public Guid RoleId { get; set; }
        public Role? Role { get; set; }
        public string? Comment { get; set; }
    }
}