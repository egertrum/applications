using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Base.Identity
{
    public class BaseAppUserRole<TKey, TAppUser, TAppRole> : IdentityUserRole<TKey> 
        where TKey : IEquatable<TKey>
        where TAppUser: IdentityUser<TKey>
        where TAppRole: IdentityRole<TKey>
    {
        public TAppUser? User { get; set; }
        public TAppRole? Role { get; set; }
    }

}