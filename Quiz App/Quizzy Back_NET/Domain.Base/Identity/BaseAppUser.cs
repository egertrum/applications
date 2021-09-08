using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Base.Identity
{
    public class BaseAppUser<TKey> : IdentityUser<TKey> 
        where TKey : IEquatable<TKey>
    {
        [MaxLength(128)]
        public virtual string FirstName { get; set; } = default!;
        
        [MaxLength(128)]
        public virtual string LastName { get; set; } = default!;

        [DataType(DataType.Date)]
        public virtual DateTime DOB { get; set; }
        

        public virtual string FirstLastName => $"{FirstName} {LastName}".Trim();
        public virtual string LastFirstName => $"{LastName} {FirstName}".Trim();

        public override string ToString() => $"{FirstName} {LastName} - {DOB.ToShortDateString()}";
    }
}