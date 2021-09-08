using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;

namespace Domain.Base
{
    public class DomainEntityMetaId: DomainEntityMetaId<Guid>, IDomainEntityId
    {
        
    }

    public class DomainEntityMetaId<TKey>: DomainEntityId<TKey> , IDomainEntityMeta
        where TKey : IEquatable<TKey>
    {
        public string? CreatedBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? UpdateBy { get; set; }
        
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    }
}