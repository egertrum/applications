using System;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class GameType: DomainEntityId
    {
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        public Guid NameId { get; set; }
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        public string Name { get; set; } = default!;

        [Display(Name = nameof(Calling), ResourceType = typeof(Base.Resources.DTO.v1.GameType))]
        public EGameTypeCalling Calling { get; set; } = default!;
    }
}