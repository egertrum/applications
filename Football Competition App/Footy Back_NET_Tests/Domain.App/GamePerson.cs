using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
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

        [DataType(DataType.Date)] 
        public DateTime From { get; set; } = DateTime.Now.Date;
        
        public DateTime? Until { get; set; }

        public string? Comment { get; set; }
    }
}