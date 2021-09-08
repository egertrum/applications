using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using PublicApi.DTO.v1;
using TestProject.Helpers;
using WebApp;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.ApiIntegrationTests
{
    public class ApiCompetitionCreateFlowTests: IClassFixture<CustomWebApplicationFactory<WebApp.Startup>>
    {
        private readonly CustomWebApplicationFactory<WebApp.Startup> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;

        private const string ApiUri = "https://localhost:5001/api/v1/";


        public ApiCompetitionCreateFlowTests(CustomWebApplicationFactory<Startup> factory,
            ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("test_database_name", Guid.NewGuid().ToString());
                })
                .CreateClient(new WebApplicationFactoryClientOptions()
                    {
                        // dont follow redirects
                        AllowAutoRedirect = false
                    }
                );

            _client.BaseAddress = new Uri(ApiUri);
            _client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        

        [Fact]
        public async Task Api_Get_Countries_Bind_To_New_Competition_And_Create_Competition()
        {
            // ARRANGE
            var c = _client;
            await LogIn();
            var uri = "Countries";
            
            // ACT
            var httpResponse = await _client.GetAsync(uri);
            httpResponse.EnsureSuccessStatusCode();
            var countries = await JsonHelper.DeserializeWithWebDefaults<List<PublicApi.DTO.v1.Country>>(httpResponse.Content);

            // ASSERT
            countries.Should().NotBeNull();

            await Bind_Country_To_New_Competition_And_Add(countries);
        }

        private async Task Bind_Country_To_New_Competition_And_Add(List<Country>? countries)
        {
            //ARRANGE
            var createUri = "Competitions";
            var countryId = countries!.FirstOrDefault()!.Id;
            var competition = new Competition()
            {
                CountryId = countryId,
                Name = "Test Cup",
                StartDate = DateTime.Now.Date,
                Organiser = "Admin"
            };
            
            //ACT
            var data = JsonHelper.SerializeWithWebDefaults(competition)!;
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(createUri, content);

            //ASSERT
            response.EnsureSuccessStatusCode();
            var obj = await JsonHelper.DeserializeWithWebDefaults<PublicApi.DTO.v1.Competition>(response.Content);
            obj.Should().NotBeNull();

            await Competitions_List_Updated();
        }
        
        private async Task Competitions_List_Updated()
        {
            //ARRANGE
            var indexUri = "Competitions";

            //ACT
            var response = await _client.GetAsync(indexUri);

            //ASSERT
            response.EnsureSuccessStatusCode();
            var obj = await JsonHelper.DeserializeWithWebDefaults<List<PublicApi.DTO.v1.Competition>>(response.Content);
            obj.Should().NotBeNull();
            obj!.Count.Should().Equals(2);
        }

        private async Task<HttpResponseMessage> LogIn()
        {
            // ARRANGE
            var login = new PublicApi.DTO.v1.Login()
            {
                Email = "admin@egrumj.com",
                Password = "Foo.bar1",
                RememberMe = true
            };

            // ACT
            var data = JsonHelper.SerializeWithWebDefaults(login)!;
            HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
            const string? loginUri = "Account/Login";

            var response = await _client.PostAsync(loginUri, content);
            response.EnsureSuccessStatusCode();
            await AddAuthHeader(response);
            return response;
        }

        private async Task AddAuthHeader(HttpResponseMessage response)
        {
            var data = await JsonHelper.DeserializeWithWebDefaults<JwtResponse>(response.Content);
            data.Should().NotBeNull();
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + data!.Token);
        }
    }

}