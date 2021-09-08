using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using BLL.Base.Services;
using Contracts.DAL.App;
using Contracts.DAL.App.Repositories;
using DAL.App.EF;
using DAL.App.EF.AppDataInit;
using DAL.App.EF.Repositories;
using Domain.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.UnitTests
{
    public class PersonBaseServiceTests
    {
        private readonly BaseEntityService<IAppUnitOfWork, IPersonRepository, BLL.App.DTO.Person, DAL.App.DTO.Person>
            _baseService;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly AppDbContext _ctx;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        // ARRANGE - common
        public PersonBaseServiceTests(ITestOutputHelper testOutputHelper)
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

            _mapper = mapperConf.CreateMapper();
            var uow = new AppUnitOfWork(_ctx, _mapper);
            var personRepository = new PersonRepository(_ctx, _mapper);
            var personMapper = new BLL.App.Mappers.PersonMapper(_mapper);

            _baseService =
                new BaseEntityService<IAppUnitOfWork, IPersonRepository, BLL.App.DTO.Person, DAL.App.DTO.Person>
                    (uow, personRepository, personMapper);

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            _logger = loggerFactory.CreateLogger<ILogger<PersonServiceTests>>();

        }

        [Fact]
        public async Task Person_Add()
        {
            // ARRANGE
            var (person, userId, countryId, dateOfBirth) = await AddData("12345123");

            // ASSERT
            Assert.NotNull(person);
            Assert.Equal(userId, person!.AppUserId);
            Assert.Equal(countryId, person!.CountryId);
            Assert.Equal(dateOfBirth, person!.BirthDate);
            Assert.Equal("Aabits", person!.FirstName);
            Assert.Equal("Kraabits", person!.LastName);
            Assert.Equal("12345123", person!.IdentificationCode);
            Assert.Equal("Man", person!.Gender);
        }

        [Fact]
        public async Task Person_Exists()
        {
            // ARRANGE
            var (person, userId, countryId, dateOfBirth) = await AddData("1");
            
            // ASSERT
            Assert.NotNull(person);
            var exists = await _baseService.ExistsAsync(person!.Id);
            Assert.True(exists);
        }
        
        
        private async Task<(Person?, Guid, Guid, DateTime)> AddData(string idCode)
        {
            // ARRANGE
            var userId = Guid.NewGuid();
            var countryId = Guid.NewGuid();
            var country = new Country()
            {
                Name = new LangString("Eesti"),
                Id = countryId
            };
            var dob = DateTime.Now.Date;
            var person = new BLL.App.DTO.Person()
            {
                AppUserId = userId,
                Country = country,
                FirstName = "Aabits",
                LastName = "Kraabits",
                IdentificationCode = idCode,
                BirthDate = dob,
                Gender = "Man"
            };
            
            // ACT
            var addedPerson = _baseService.Add(person);
            await _ctx.SaveChangesAsync();

            return (await _baseService.FirstOrDefaultAsync(addedPerson.Id), userId, countryId, dob);
        }
        
        [Fact]
        public async Task First_Or_Default()
        {
            // ARRANGE
            var (personEntity, userId, countryId, dateOfBirth) = await AddData("1");
            Assert.NotNull(personEntity);
            
            var person = await _baseService.FirstOrDefaultAsync(personEntity!.Id);
            
            //ASSERT
            Assert.NotNull(person);
            Assert.Equal(userId, person!.AppUserId);
            Assert.Equal(countryId, person!.CountryId);
            Assert.Equal(dateOfBirth, person!.BirthDate);
            Assert.Equal("Aabits", person!.FirstName);
            Assert.Equal("Kraabits", person!.LastName);
            Assert.Equal("1", person!.IdentificationCode);
            Assert.Equal("Man", person!.Gender);
        }
        
        [Fact]
        public async Task Get_All_Persons()
        {
            // ARRANGE
            for (int i = 0; i < 6; i++)
            {
                await AddData(i.ToString());
            }
            var persons = (await _baseService.GetAllAsync()).ToList();
            
            // ASSERT
            Assert.NotNull(persons);
            Assert.Equal(6, persons.Count);
        }
        
        [Fact]
        public async Task Remove_Person()
        {
            // ARRANGE
            var (person, userId, countryId, dateOfBirth) = await AddData("12345123");
            for (int i = 0; i < 6; i++)
            {
                await AddData(i.ToString());
            }
            _ctx.ChangeTracker.Clear();
            
            //ACT
            var persons = (await _baseService.GetAllAsync()).ToList();
            Assert.NotNull(persons);
            Assert.Equal(7, persons.Count);

            await _baseService.RemoveAsync(person!.Id, person!.AppUserId);
            await _ctx.SaveChangesAsync();
            
            //ASSERT
            var personsAfterRemove = (await _baseService.GetAllAsync()).ToList();
            Assert.Equal(6, personsAfterRemove.Count);
        }
        
        [Fact]
        public async Task Update_Person()
        {
            // ARRANGE
            var (person, userId, countryId, dateOfBirth) = await AddData("12345123");
            _ctx.ChangeTracker.Clear();
            Assert.NotNull(person);
            person!.FirstName = "Andrus";
            
            // ACT
            var updatedPerson = _baseService.Update(person);
            await _ctx.SaveChangesAsync();

            // ASSERT
            Assert.NotEqual("Aabits", updatedPerson.FirstName);
            Assert.Equal("Andrus", updatedPerson.FirstName);
        }
    }
}