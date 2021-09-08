using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Country: DomainEntityId
    {
        public Guid NameId { get; set; }
        [MaxLength(256)] public string Name { get; set; } = default!;
        
        public ICollection<Competition>? Competitions { get; set; }
        
        public ICollection<Person>? Persons { get; set; }
        
        public ICollection<Team>? Teams { get; set; }
    }
}