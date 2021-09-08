using System;
using System.Collections.Generic;
using Domain.Base;

namespace BLL.App.DTO
{
    public class GameType: DomainEntityId
    {
        public Guid NameId { get; set; }
        public string Name { get; set; } = default!;

        public EGameTypeCalling Calling { get; set; }
        
        public ICollection<Game>? Games { get; set; }
    }
}