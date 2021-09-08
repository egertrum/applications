using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using BLL.App.Services;
using DAL.App.EF;
using DAL.App.EF.AppDataInit;
using Domain.App;
using Domain.App.Identity;
using Domain.Base;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;
using Xunit;
using Xunit.Abstractions;
using WebApp.ViewModels.Competitions;
using Country = Domain.App.Country;
using GamePartType = Domain.App.GamePartType;
using Person = Domain.App.Person;
using Team = Domain.App.Team;
using TeamPerson = Domain.App.TeamPerson;

namespace TestProject.UnitTests
{
    public class PersonServiceTests
    {
        private readonly PersonService _personService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly AppDbContext _ctx;
        private readonly ILogger _logger;
        /*
        private readonly Mock<HttpClientAdapter> _adapter;
        */
        
        // ARRANGE - common
        public PersonServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            // set up db context for testing - using InMemory db engine
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // provide new random database name here
            // or parallel test methods will conflict each other
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _ctx = new AppDbContext(optionBuilder.Options);
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();
            
            var mapperConf = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DAL.App.DTO.MappingProfiles.AutoMapperProfile>(); 
                cfg.AddProfile<BLL.App.DTO.MappingProfiles.AutoMapperProfile>(); 
                cfg.AddProfile<PublicApi.DTO.v1.MappingProfiles.AutoMapperProfile>(); 

            });
            var mapper = mapperConf.CreateMapper();

            var uow = new AppUnitOfWork(_ctx, mapper);
            

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<CompetitionsController>();
            
            // SUT
            _personService = new PersonService(uow, uow.Persons, mapper);
        }


        [Fact]
        public async Task Person_Does_Not_Exist_For_UserId_By_Identification_Code_When_Code_Is_Empty_String()
        {
            // ACT
            var result = await _personService.ExistsByIdentificationCode("", Guid.Empty);
            
            // ASSERT
            Assert.False(result);
        }
        
        [Fact]
        public async Task Person_Exists_For_UserId_By_Identification_Code()
        {
            // ARRANGE
            var (identificationCode, userId, personId) = await SeedData();
            
            // ACT
            var result = await _personService.ExistsByIdentificationCode(identificationCode, userId);
            
            // ASSERT
            Assert.True(result);
        }
        
        [Fact]
        public async Task Person_Does_Not_Exist_For_UserId_By_Identification_Code_When_UserId_Is_Not_Correct()
        {
            // ARRANGE
            var wrongUserId = Guid.NewGuid();
            var (identificationCode, userId, personId) = await SeedData();
            
            // ACT
            var result = await _personService.ExistsByIdentificationCode(identificationCode, wrongUserId);
            
            // ASSERT
            Assert.False(result);
        }
        
        [Fact]
        public async Task Get_Person_By_IdentificationCode_Is_Null_When_UserId_Not_Existing()
        {
            // ARRANGE
            var wrongUserId = Guid.NewGuid();
            var (identificationCode, userId, personId) = await SeedData();
            
            // ACT
            var result = await _personService.FindByIdentificationCode(identificationCode, wrongUserId);
            
            // ASSERT
            Assert.Null(result);
        }
        
        [Fact]
        public async Task Get_Person_By_IdentificationCode()
        {
            // ARRANGE
            var (identificationCode, userId, personId) = await SeedData();
            
            // ACT
            var result = await _personService.FindByIdentificationCode(identificationCode, userId);
            
            // ASSERT
            Assert.NotNull(result);
            Assert.Equal(identificationCode, result!.IdentificationCode);
            Assert.Equal(userId, result!.AppUserId);
        }
        
        //[Theory]
        //[ClassData(typeof(CountGenerator))]
        [Fact]
        public async Task Get_Person_Teams()
        {
            // ARRANGE
            //var teamsCount = count;
            var teamsCount = 5;
            var (identificationCode, userId, personId) = await SeedData(teamsCount);
            
            // ACT
            var result = await _personService.FirstOrDefaultAsyncWithTeams(personId);
            
            // ASSERT
            Assert.NotNull(result);
            Assert.IsType<BLL.App.DTO.Person>(result);
            result!.PersonTeams
                .Should().NotBeNull()
                .And.HaveCount(teamsCount);
        }

        private async Task<(string, Guid, Guid)> SeedData(int teams = 0, int count = 1)
        {
            var userId = Guid.NewGuid();
            var countryId = Guid.NewGuid();
            var user = new AppUser()
            {
                IdentificationCode = "0",
                Id = userId
            };
            await _ctx.Users.AddAsync(user);
            
            var country = new Country()
            {
                Id = countryId,
                Name = "Eesti",
                NameId = new Guid()
            };
            await _ctx.Countries.AddAsync(country);
            
            var personId = Guid.NewGuid();
            for (int i = 0; i < count; i++)
            {
                var person = new Person()
                {
                    FirstName = "person" + i,
                    LastName = "person last" + i,
                    AppUserId = userId,
                    CountryId = countryId,
                    BirthDate = DateTime.Now,
                    IdentificationCode = i.ToString(),
                };
                person.Id = count == 1 ? personId : Guid.NewGuid();
                await _ctx.Persons.AddAsync(person);
            }

            for (int i = 0; i < teams; i++)
            {
                var teamId = Guid.NewGuid();
                var team = new Team()
                {
                    Id = teamId,
                    Name = "team" + i,
                    AppUserId = userId,
                    CountryId = countryId,
                };
                await _ctx.Teams.AddAsync(team);
                await _ctx.SaveChangesAsync();
                var teamPerson = new TeamPerson()
                {
                    TeamId = teamId,
                    PersonId = personId
                };
                await _ctx.TeamPersons.AddAsync(teamPerson);
            }
            
            await _ctx.SaveChangesAsync();
            return (user.IdentificationCode, userId, personId);
        }
    }
    
    public class CountGenerator : IEnumerable<object[]>
    {
        private static List<object[]> _data 
        {
            get
            {
                var res = new List<Object[]>();
                for (int i = 1; i <= 100; i++)
                {
                    res.Add(new object[]{i});
                }

                return res;
            }
        }
        
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
