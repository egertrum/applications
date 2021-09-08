using System;
using System.Collections.Generic;
using System.Linq;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DAL.App.EF.AppDataInit
{
    public static class DataInit
    {
        static readonly string[] SomeCountries =
            {"Finland", "Latvia", "Lithuania", "Sweden", "Norway", "Russia", "England"};

        static readonly Dictionary<string, EGamePartType> SomeGamePartTypes =
            new Dictionary<string, EGamePartType>(){
                {"First half", EGamePartType.FirstHalf},
                {"Second half", EGamePartType.SecondHalf},
                {"Additional first half", EGamePartType.ExtraTimeFirstHalf},
                {"Additional second half", EGamePartType.ExtraTimeSecondHalf},
                {"Penalties", EGamePartType.Penalties}
            }; 
        
        static readonly string[] SomeRoles =
            {"Player", "Coach"};
        
        static readonly Dictionary<string, EGameTypeCalling> GameTypes =  
            new Dictionary<string, EGameTypeCalling>(){
                {"Two halves", EGameTypeCalling.Normal},
                {"Two halves + Two additional halves", EGameTypeCalling.Additional},
                {"Two halves + Two additional halves + Penalties", EGameTypeCalling.Penalties} }; 
        
        public static void DropDatabase(AppDbContext ctx, ILogger logger)
        {
            logger.LogInformation("DropDatabase");
            ctx.Database.EnsureDeleted();
        }

        public static void MigrateDatabase(AppDbContext ctx, ILogger logger)
        {
            logger.LogInformation("MigrateDatabase");
            ctx.Database.Migrate();
        }
        
        public static void SeedAppData(AppDbContext ctx, ILogger logger)
        {
            logger.LogInformation("SeedAppInitialData");
            foreach (var name in SomeCountries)
            {
                Country c = new() {Name = name};
                ctx.Countries.Add(c);   
            }
            
            foreach (KeyValuePair<string, EGamePartType> gamePartType in SomeGamePartTypes)
            {
                GamePartType g = new() {Name = gamePartType.Key, Short = gamePartType.Value};
                ctx.GamePartTypes.Add(g);   
            }
            
            foreach (var rName in SomeRoles)
            {
                Role r = new() {Name = rName};
                ctx.PersonRoles.Add(r);   
            }
            
            foreach(KeyValuePair<string, EGameTypeCalling> gameType in GameTypes)
            {
                GameType g = new() {Name = gameType.Key, Calling = gameType.Value};
                ctx.GameTypes.Add(g);
            }
            
            var user = ctx.Users.FirstOrDefault(x => x.Firstname == "Admin");

            var country = new Country()
            {
                Name = "Estonia"
            };
            ctx.Countries.Add(country);
            
            var person = new Person()
            {
                Country = country,
                FirstName = user!.Firstname,
                LastName = user!.Lastname,
                IdentificationCode = user!.IdentificationCode,
                AppUserId = user.Id,
                BirthDate = DateTime.Now
            };
            ctx.Persons.Add(person);
            ctx.SaveChanges();

            var competition = new Competition()
            {
                Name = "Admin Cup",
                CountryId = country.Id,
                StartDate = DateTime.Now,
                Organiser = "Admin"
            };
            ctx.Competitions.Add(competition);
            ctx.SaveChanges();
        }

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration? configuration, ILogger logger, AppDbContext ctx)
        {
            var admin = CreateRole("Admin", roleManager, logger);
            var footyUser = CreateRole("FootyUser", roleManager, logger);

            var user = new AppUser();
            var password = "";
            if (configuration != null)
            {
                password = configuration["AdminUser:Password"];
                user.Email = configuration["AdminUser:Email"];
                user.Firstname = configuration["AdminUser:Firstname"];
                user.Lastname = configuration["AdminUser:Lastname"];
                user.UserName = user.Email;
                user.IdentificationCode = configuration["AdminUser:IdentificationCode"];   
            }
            else
            {
                password = "Foo.bar1";
                user.Email = "admin@egrumj.com";
                user.Firstname = "Admin";
                user.Lastname = "egrumj";
                user.UserName = "adminegrumj";
                user.IdentificationCode = "1";   
            }

            var result = userManager.CreateAsync(user, password).Result;
            if (!result.Succeeded)
            {
                throw new ApplicationException("User creation failed");
            }

            logger.LogInformation("AddAdminUser");


            result = userManager.AddToRoleAsync(user, admin.Name).Result;
            
            if (!result.Succeeded)
            {
                throw new ApplicationException("Role adding to user failed");
            }
            
            logger.LogInformation("AddAdminRoleToAdmin");
        }

        private static AppRole CreateRole(string roleName, RoleManager<AppRole> roleManager, ILogger logger)
        {
            var role = new AppRole();
            role.Name = roleName;
            role.DisplayName = roleName;
            
            var result = roleManager.CreateAsync(role).Result;
            
            if (!result.Succeeded)
            {
                throw new ApplicationException("Role creation failed");
            }
            
            logger.LogInformation("AddRoleToDatabase");

            return role;
        }
    }
}
