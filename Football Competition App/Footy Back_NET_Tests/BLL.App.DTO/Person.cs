using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Person : DomainEntityId, IDomainAppUser<AppUser>, IDomainAppUserId<Guid>
    {
        public Guid CountryId { get; set;}
        public Country? Country { get; set; }
        
        [MaxLength(128)] public string FirstName { get; set; } = default!;
        
        [MaxLength(128)] public string LastName { get; set; } = default!;
        
        public string Name => FirstName + " " + LastName;

        public string IdentificationCode { get; set; } = default!;

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        
        [MaxLength(16)] public string? Gender { get; set; }
        
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        public IDictionary<string, int>? PersonTeams { get; set; }
        
        public ICollection<TeamPerson>? TeamPersons { get; set; }
        
        public ICollection<Registration>? Registrations { get; set; }
        
        public ICollection<Participation>? Participations { get; set; }

        public ICollection<GamePerson>? GamePersons { get; set; }
    }
}