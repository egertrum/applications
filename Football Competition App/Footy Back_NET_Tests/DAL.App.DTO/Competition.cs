using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using DAL.App.DTO.Identity;
using Domain.Base;

namespace DAL.App.DTO
{
    public class Competition: DomainEntityId
    {
        public Guid CountryId { get; set;}
        public Country? Country { get; set; }
        
        [MaxLength(256)] public string Name { get; set; } = default!;
        
        [MaxLength(128)] public string Organiser { get; set; } = default!;

        [DataType(DataType.Date)] public DateTime StartDate { get; set; }
            
        [DataType(DataType.Date)] public DateTime? EndDate { get; set; }

        [MaxLength(Int32.MaxValue)] public string? Comment { get; set; }
        
        public ICollection<CompetitionTeam>? CompetitionTeams { get; set; }
        
        public ICollection<Game>? Games { get; set; }
        
        public ICollection<Registration>? Registrations { get; set; }
    }
}