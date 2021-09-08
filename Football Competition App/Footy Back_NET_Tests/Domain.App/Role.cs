using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Role: DomainEntityId
    {
        public Guid NameId { get; set; }
        public LangString? Name { get; set; }
        
        public Guid? CommentId { get; set; }
        public LangString? Comment { get; set; }
        
        public ICollection<TeamPerson>? TeamPersons { get; set; }
        
        public ICollection<Participation>? Participators { get; set; }
        
        public ICollection<GamePerson>? GamePersons { get; set; }
    }
}