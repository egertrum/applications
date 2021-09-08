using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using TestProject.Helpers;
using WebApp;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.IntegrationTests
{
    public class MainActionsWithAuthenticationNeeds: IClassFixture<CustomWebApplicationFactory<WebApp.Startup>>
    {
        private readonly CustomWebApplicationFactory<WebApp.Startup> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;


        public MainActionsWithAuthenticationNeeds(CustomWebApplicationFactory<Startup> factory,
            ITestOutputHelper testOutputHelper)
        {
            _factory = factory;
            _testOutputHelper = testOutputHelper;
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseSetting("test_database_name", 
                        Guid.NewGuid().ToString());
                })
                .CreateClient(new WebApplicationFactoryClientOptions()
                    {
                        // dont follow redirects
                        AllowAutoRedirect = false
                    }
                );
        }

        [Fact]
        public async Task Register_To_Competition_WithOut_Auth_Redirects_And_Registers()
        {
            // ARRANGE
            var uri = "/CompetitionTeams/Create";

            // ACT
            var getTestResponse = await _client.GetAsync(uri);
            
            // ASSERT
            Assert.Equal(302, (int) getTestResponse.StatusCode);
            var redirectUri = getTestResponse.Headers.FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();
            redirectUri.Should().NotBeNull();
            await Get_Login_Page(redirectUri!);
        }

        [Fact]
        public async Task CompetitionsCreateAuth_AuthRedirect()
        {
            // ARRANGE
            var uri = "/Competitions/Create";
            
            // ACT
            var getTestResponse = await _client.GetAsync(uri);
            
            // ASSERT
            Assert.Equal(302, (int) getTestResponse.StatusCode);
        }
        
        [Fact]
        public async Task CompetitionsCreateAuth_AuthFlow()
        {
            // ARRANGE
            var uri = "/Competitions/Create";
            
            // ACT
            var getTestResponse = await _client.GetAsync(uri);
            
            // ASSERT
            Assert.Equal(302, (int) getTestResponse.StatusCode);
            var redirectUri = getTestResponse.Headers.FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();
            redirectUri.Should().NotBeNull();
            
            await Get_Login_Page(redirectUri!);
        }

        public async Task Get_Login_Page(string uri)
        {
            var getLoginPageResponse = await _client.GetAsync(uri);
            getLoginPageResponse.EnsureSuccessStatusCode();
            
            // get the document
            var getLoginDocument = await HtmlHelpers.GetDocumentAsync(getLoginPageResponse);
            
            var registerAnchorElement = (IHtmlAnchorElement) getLoginDocument.QuerySelector("#register");
            var registerUrl = registerAnchorElement.Href;
            _testOutputHelper.WriteLine("Register url: " + registerUrl);

            await Get_Register_Page(registerUrl);
        }

        public async Task Get_Register_Page(string uri)
        {
            // ARRANGE
            var getRegisterPageResponse = await _client.GetAsync(uri);
            getRegisterPageResponse.EnsureSuccessStatusCode();
            
            // get the document
            // ACT
            var getRegisterDocument = await HtmlHelpers.GetDocumentAsync(getRegisterPageResponse);
            var regForm = (IHtmlFormElement) getRegisterDocument.QuerySelector("#register-form"); 
            var countryId = ((IHtmlOptionElement) getRegisterDocument.QuerySelector("#Input_CountryId").Children[1]).Value;
            var regFormValues = new Dictionary<string, string>()
            {
                ["Input_Email"] = "test@test.ee",
                ["Input_Password"] = "Foo.bar1",
                ["Input_ConfirmPassword"] = "Foo.bar1",
                ["Input_FirstName"] = "test",
                ["Input_LastName"] = "test",
                ["Input_Gender"] = "Man",
                ["Input_CountryId"] = countryId,
                ["Input_IdentificationCode"] = "54600000",
            };

            var regPostResponse = await _client.SendAsync(regForm, regFormValues);
            _testOutputHelper.WriteLine("Register statuscode: " + regPostResponse.StatusCode);

            // ASSERT
            regPostResponse.StatusCode.Should().Equals(302);
            
            var redirectUri = regPostResponse.Headers.FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();
            redirectUri.Should().NotBeNull();
            
            await Get_User_Authenticated(redirectUri!);
        }

        public async Task Get_User_Authenticated(string uri)
        {
            // ARRANGE
            var getTestResponse = await _client.GetAsync(uri);
            getTestResponse.EnsureSuccessStatusCode();
            
            _testOutputHelper.WriteLine($"Uri '{uri}' was accessed with response status code '{getTestResponse.StatusCode}'.");

            await Competition_Create_Page_Success_While_Authenticated();
        }
        
        public async Task Competition_Create_Page_Success_While_Authenticated()
        {
            // ARRANGE
            var uri = "/Competitions/Create";
            
            // ACT
            var getCompetitionCreateResponse = await _client.GetAsync(uri);
            
            // ASSERT
            Assert.Equal(200, (int) getCompetitionCreateResponse.StatusCode);
        }
        
        [Fact]
        public async Task Register_Unmatching_Password_And_No_Country_Is_Bad_Request()
        {
            // ARRANGE
            var uri = "/Identity/Account/Register";
            var getRegisterPageResponse = await _client.GetAsync(uri);
            getRegisterPageResponse.EnsureSuccessStatusCode();
            
            // get the document
            // ACT
            var getRegisterDocument = await HtmlHelpers.GetDocumentAsync(getRegisterPageResponse);
            var regForm = (IHtmlFormElement) getRegisterDocument.QuerySelector("#register-form"); 
            var regFormValues = new Dictionary<string, string>()
            {
                ["Input_Email"] = "test@test.ee",
                ["Input_Password"] = "Foo.bar1",
                ["Input_ConfirmPassword"] = "Bar.Foo2",
                ["Input_FirstName"] = "test",
                ["Input_LastName"] = "test",
                ["Input_Gender"] = "Man",
                ["Input_IdentificationCode"] = "54600000",
            };

            var regPostResponse = await _client.SendAsync(regForm, regFormValues);
            // ASSERT
            regPostResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}