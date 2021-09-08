using System;
using System.Collections.Generic;
using Domain.Base;

namespace BLL.App.DTO
{
    public class Game: DomainEntityId
    {
        public Guid HomeId { get; set; }
        public Team? Home { get; set; }

        public Guid AwayId { get; set; }
        public Team? Away { get; set; }
        
        public Guid CompetitionId { get; set; }
        public Competition? Competition { get; set; }
        
        public Guid GameTypeId { get; set; }
        public GameType? GameType { get; set; }

        public string Name => Home?.Name + " vs " + Away?.Name;
        
        public string Score => HomeScore + " - " + AwayScore;

        public DateTime? KickOffTime { get; set; }

        public int? HomeScore { get; set; }
        
        public int? AwayScore { get; set; }
        
        public string? Comment { get; set; }

        public Guid? IdOfGameWinner { get; set; }
        
        public ICollection<Event>? Events { get; set; }
        
        public ICollection<GamePart>? GameParts { get; set; }
        
        public ICollection<GamePerson>? GamePersons { get; set; }
    }
}