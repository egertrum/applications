using System;
using System.Linq;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Base.EF
{
    public class BaseDbContext<TUser, TRole, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> :
        BaseDbContext<TUser, TRole, Guid, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : BaseAppUser<Guid>
        where TRole : BaseAppRole<Guid>
        where TUserClaim : IdentityUserClaim<Guid>
        where TUserRole : BaseAppUserRole<Guid, TUser, TRole>
        where TUserLogin : IdentityUserLogin<Guid>
        where TRoleClaim : IdentityRoleClaim<Guid>
        where TUserToken : IdentityUserToken<Guid>
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class BaseDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> :
        IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : BaseAppUser<TKey>
        where TRole : BaseAppRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : BaseAppUserRole<TKey, TUser, TRole>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // disable cascade delete initially for everything
            foreach (var relationship in builder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<BaseAppUserRole<TKey, TUser, TRole>>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);
            
            builder.Entity<BaseAppUserRole<TKey, TUser, TRole>>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);
        }
    }
}