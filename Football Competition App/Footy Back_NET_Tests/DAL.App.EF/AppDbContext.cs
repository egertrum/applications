using System;
using System.Linq;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<Competition> Competitions { get; set; } = default!;
        public DbSet<Person> Persons { get; set; } = default!;
        public DbSet<Country> Countries { get; set; } = default!;
        public DbSet<Registration> Registrations { get; set; } = default!;
        public DbSet<CompetitionTeam> CompetitionTeams { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<Game> Games { get; set; } = default!;
        public DbSet<GamePart> GameParts { get; set; } = default!;
        public DbSet<GameType> GameTypes { get; set; } = default!;
        public DbSet<GamePartType> GamePartTypes { get; set; } = default!;
        public DbSet<GamePerson> GamePersons { get; set; } = default!;
        public DbSet<Participation> Participators { get; set; } = default!;
        public DbSet<Role> PersonRoles { get; set; } = default!;
        public DbSet<Team> Teams { get; set; } = default!;
        public DbSet<TeamPerson> TeamPersons { get; set; } = default!;
        public DbSet<Report> Reports { get; set; } = default!;
        
        public DbSet<LangString> LangStrings { get; set; } = default!;
        public DbSet<Translation> Translations { get; set; } = default!;

        
        
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Translation>().HasKey(k => new { k.Culture, k.LangStringId});
            
            // disable cascade delete initially for everything
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            /*
            builder.Entity<Contact>()
                .HasIndex(x => new {x.PersonId, x.ContactTypeId})
                .IsUnique();
                
            builder.Entity<ContactType>()
                .HasMany(m => m.Contacts)
                .WithOne(o => o.ContactType!)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ContactType>()
                .HasMany(m => m.SecondaryContacts)
                .WithOne(o => o.SecondaryContactType!)
                .OnDelete(DeleteBehavior.Cascade);
            */
        }
    }

}