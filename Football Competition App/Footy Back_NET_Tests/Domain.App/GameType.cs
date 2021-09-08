using System;
using System.Collections.Generic;
using Domain.Base;

namespace Domain.App
{
    public class GameType: DomainEntityId
    {
        public Guid NameId { get; set; }
        public LangString? Name { get; set; }

        public EGameTypeCalling Calling { get; set; }
        
        public ICollection<Game>? Games { get; set; }
    }
}