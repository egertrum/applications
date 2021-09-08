using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Base.Identity
{
    public class BaseAppUser<TKey> : IdentityUser<TKey> 
        where TKey : IEquatable<TKey>
    {
        [StringLength(128, MinimumLength = 1)]
        public virtual string Firstname { get; set; } = default!;
        [StringLength(128, MinimumLength = 1)]
        public virtual string Lastname { get; set; } = default!;
        
        public virtual string IdentificationCode { get; set; } = default!;
        public virtual string FullName => Firstname + " " + Lastname;
        public virtual string FullNameEmail => FullName + " (" + Email + ")";
        
        public override string ToString() => $"{Firstname} {Lastname} - {IdentificationCode}";
    }

}