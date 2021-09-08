using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Role: DomainEntityId
    {
        public Guid NameId { get; set; }
        [MaxLength(128)] public string Name { get; set; } = default!;
        
        public Guid? CommentId { get; set; }
        [MaxLength(Int32.MaxValue)] public string? Comment { get; set; }
        
        public ICollection<TeamPerson>? TeamPersons { get; set; }
        
        public ICollection<Participation>? Participators { get; set; }
        
        public ICollection<GamePerson>? GamePersons { get; set; }
    }
}