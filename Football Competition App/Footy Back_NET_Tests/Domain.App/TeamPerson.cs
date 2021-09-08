using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class TeamPerson: DomainEntityId
    {
        public Guid TeamId { get; set;}
        public Team? Team { get; set; }
        public Guid PersonId { get; set;}
        public Person? Person { get; set; }
        public Guid RoleId { get; set;}
        public Role? Role { get; set; }
        
        public DateTime Since { get; set; }

        public DateTime? Until { get; set; }
    }
}