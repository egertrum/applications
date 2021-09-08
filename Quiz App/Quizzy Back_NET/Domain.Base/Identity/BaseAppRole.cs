using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Base.Identity
{
    public class BaseAppRole<TKey> : IdentityRole<TKey> 
        where TKey : IEquatable<TKey>
    {
        [MaxLength(128)] 
        public virtual string DisplayName { get; set; } = default!;

        public override string ToString() => DisplayName;
    }
}