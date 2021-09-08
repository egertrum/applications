using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using TestProject.Helpers;
using WebApp;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.IntegrationTests
{
    public class MainActionsWithoutAuthentication : IClassFixture<CustomWebApplicationFactory<WebApp.Startup>>
    {
        private readonly CustomWebApplicationFactory<WebApp.Startup> _factory;
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _testOutputHelper;


        public MainActionsWithoutAuthentication(CustomWebApplicationFactory<Startup> factory,
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
        public async Task Competitions_HasSuccessStatusCode()
        {
            // ARRANGE
            var uri = "/Competitions";

            // ACT
            var getCompetitionsResponse = await _client.GetAsync(uri);

            // ASSERT
            getCompetitionsResponse.EnsureSuccessStatusCode();
            Assert.InRange((int) getCompetitionsResponse.StatusCode, 200, 299);
        }

        [Fact]
        public async Task Games_HasSuccessStatusCode()
        {
            // ARRANGE
            var uri = "/Games";

            // ACT
            var getGamesResponse = await _client.GetAsync(uri);

            // ASSERT
            getGamesResponse.EnsureSuccessStatusCode();
            Assert.InRange((int) getGamesResponse.StatusCode, 200, 299);
        }
        
        [Fact]
        public async Task Get_Competition_Info()
        {
            // ARRANGE
            var uri = "/Competitions";
            var getCompetitionsResponse = await _client.GetAsync(uri);
            getCompetitionsResponse.EnsureSuccessStatusCode();
            
            // ACT
            var getCompetitionsDocument = await HtmlHelpers.GetDocumentAsync(getCompetitionsResponse);
            var competitionsBody = (IHtmlTableSectionElement) getCompetitionsDocument.QuerySelector("#competitions");

            // ASSERT
            competitionsBody.ChildElementCount.Should().Equals(1);

            await Get_Competition_Details(competitionsBody);
        }
        
        public async Task Get_Competition_Details(IHtmlTableSectionElement competitionsBody)
        {
            // ARRANGE
            var test = competitionsBody.Children[0];
            var detailsAnchor = (IHtmlAnchorElement) competitionsBody.Children[0].Children[3].Children[0];
            var uri = detailsAnchor.Href;
            // ACT
            var getCompetitionDetails = await _client.GetAsync(uri);
            
            // ASSERT
            Assert.Equal(200, (int) getCompetitionDetails.StatusCode);

            await Available_Attending_Teams_Section_And_Games_Section(getCompetitionDetails);
        }
        
        public async Task Available_Attending_Teams_Section_And_Games_Section(HttpResponseMessage getCompetitionDetails)
        {
            // ARRANGE
            var getDetailsDocument = await HtmlHelpers.GetDocumentAsync(getCompetitionDetails);
            
            // ACT
            var teamsBody = (IHtmlTableSectionElement) getDetailsDocument.QuerySelector("#attending-teams");
            var gamesElement = getDetailsDocument.QuerySelector("#games-table");

            // ASSERT
            teamsBody.ChildElementCount.Should().Equals(1);
            gamesElement.Should().NotBeNull();
        }
        
        [Fact]
        public async Task Report_Problem_With_Success()
        {
            // ARRANGE
            var uri = "/Reports/Create";
            var getReportCreateResponse = await _client.GetAsync(uri);
            getReportCreateResponse.EnsureSuccessStatusCode();
            
            // get the document
            // ACT
            var getReportCreateDocument = await HtmlHelpers.GetDocumentAsync(getReportCreateResponse);
            var reportForm = (IHtmlFormElement) getReportCreateDocument.QuerySelector("#report-form"); 
            var reportValues = new Dictionary<string, string>()
            {
                ["Title"] = "test problem",
                ["Comment"] = "deadline is approaching too fast"
            };

            var regPostResponse = await _client.SendAsync(reportForm, reportValues);

            // ASSERT
            regPostResponse.StatusCode.Should().Equals(302);
            
            var redirectUri = regPostResponse.Headers.FirstOrDefault(x => x.Key == "Location").Value.FirstOrDefault();
            redirectUri.Should().NotBeNull();
        }
    }
}