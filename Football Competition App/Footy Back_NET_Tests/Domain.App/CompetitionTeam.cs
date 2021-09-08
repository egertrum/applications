using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Domain.Base;

namespace Domain.App
{
    public class CompetitionTeam: DomainEntityId
    {
        public Guid TeamId { get; set;}
        public Team? Team { get; set; }
        
        public Guid CompetitionId { get; set;}
        public Competition? Competition { get; set; }

        [DataType(DataType.Date)] 
        public DateTime Since { get; set; } = DateTime.Now.Date;
        
        [DataType(DataType.Date)] 
        public DateTime? Until { get; set; }
    }
}