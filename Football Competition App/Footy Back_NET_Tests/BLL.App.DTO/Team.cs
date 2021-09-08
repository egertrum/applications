using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLL.App.DTO.Identity;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Team: DomainEntityId
    {
        public Guid CountryId { get; set;}
        public Country? Country { get; set; }
        
        public Guid AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        
        [MaxLength(256)] public string Name { get; set; } = default!;
        
        public int PlayersAmount { get; set; }

        public ICollection<Registration>? Registrations { get; set; }
        
        public ICollection<CompetitionTeam>? CompetitionTeams { get; set; }

        [InverseProperty(nameof(Game.Home))] public ICollection<Game>? HomeTeamGames { get; set; }
        
        [InverseProperty(nameof(Game.Away))] public ICollection<Game>? AwayTeamGames { get; set; }
        
        public ICollection<TeamPerson>? TeamPersons { get; set; }
        
        public ICollection<Participation>? Participators { get; set; }
        
        public ICollection<GamePerson>? GamePersons { get; set; }


    }
}