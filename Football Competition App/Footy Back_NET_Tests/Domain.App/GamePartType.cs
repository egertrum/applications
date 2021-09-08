using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.App
{
    public class GamePartType: DomainEntityId
    {
        public Guid NameId { get; set; }
        public LangString? Name { get; set; }

        public EGamePartType Short { get; set; }

        public ICollection<GamePart>? GameParts { get; set; }
    }
}