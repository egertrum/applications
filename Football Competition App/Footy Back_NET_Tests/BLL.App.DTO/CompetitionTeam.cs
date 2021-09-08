using System;
using Domain.Base;

namespace BLL.App.DTO
{
    public class CompetitionTeam: DomainEntityId
    {
        public Guid TeamId { get; set;}
        public Team? Team { get; set; }
        public Guid CompetitionId { get; set;}
        public Competition? Competition { get; set; }

        public DateTime Since { get; set; } = DateTime.Now;

        public DateTime? Until { get; set; }
    }
}