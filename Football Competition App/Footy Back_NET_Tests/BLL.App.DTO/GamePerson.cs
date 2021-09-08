using System;
using Domain.Base;

namespace BLL.App.DTO
{
    public class GamePerson: DomainEntityId
    {
        public Guid GameId { get; set;}
        public Game? Game { get; set; }
        
        public Guid RoleId { get; set;}
        public Role? Role { get; set; }
        
        public Guid PersonId { get; set;}
        public Person? Person { get; set; }
        
        public Guid? TeamId { get; set;}
        public Team? Team { get; set; }

        public DateTime? From { get; set; }
        
        public DateTime? Until { get; set; }

        public string? Comment { get; set; }
    }
}