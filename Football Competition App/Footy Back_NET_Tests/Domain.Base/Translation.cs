using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;

namespace Domain.Base
{

    public class Translation : Translation<Guid>
    {
    }

    public class Translation<TKey>
        where TKey: IEquatable<TKey>
    {
        [MaxLength(5)]
        public virtual string Culture { get; set; } = default!;

        [MaxLength(10240)]
        public virtual string Value { get; set; } = "";

        public TKey LangStringId { get; set; } = default!;
        public LangString? LangString { get; set; }
    }

}