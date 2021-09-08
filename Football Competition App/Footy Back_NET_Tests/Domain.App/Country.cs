using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class Country: DomainEntityId
    {
        public Guid NameId { get; set; }
        public LangString? Name { get; set; }
        
        public ICollection<Competition>? Competitions { get; set; }
        
        public ICollection<Person>? Persons { get; set; }
        
        public ICollection<Team>? Teams { get; set; }
    }
}