using System;
using System.Linq;
using DAL.Base.EF;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using Domain.Base.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF
{
    public class AppDbContext : BaseDbContext<
        AppUser, 
        AppRole, 
        IdentityUserClaim<Guid>, 
        BaseAppUserRole<Guid, AppUser, AppRole>, 
        IdentityUserLogin<Guid>, 
        IdentityRoleClaim<Guid>, 
        IdentityUserToken<Guid>>
    {
        
        public DbSet<Poll> Poll { get; set; } = default!;
        public DbSet<PollQuestion> PollQuestion { get; set; } = default!;
        public DbSet<Question> Question { get; set; } = default!;
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; } = default!;
        public DbSet<Quiz> Quiz { get; set; } = default!;
        public DbSet<QuizQuestion> QuizQuestion { get; set; } = default!;
        public DbSet<UserAnswer> UserAnswer { get; set; } = default!;
        
        public DbSet<Score> Score { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
        
    }
}