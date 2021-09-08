using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO;
using Domain.Base;

namespace PublicApi.DTO.v1
{
    public class GamePartType: DomainEntityId
    {
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        public Guid NameId { get; set; }
        [Display(Name = nameof(Name), ResourceType = typeof(Base.Resources.DTO.v1.Person))]
        [MaxLength(128)]public string Name { get; set; } = default!;
        
        [Display(Name = nameof(Short), ResourceType = typeof(Base.Resources.DTO.v1.GamePart))]
        public EGamePartType Short { get; set; } = default!;
    }
}