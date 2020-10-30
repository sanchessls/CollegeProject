using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using RazorPagesProject.Tests.Helpers;
using ScrumPokerPlanning.Context;
using Microsoft.AspNetCore.Html;

namespace RazorPagesProject.Tests
{
    #region snippet1
    public class CreateSessionPageTests : 
        IClassFixture<CustomWebApplicationFactory<ScrumPokerPlanning.Startup>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<ScrumPokerPlanning.Startup> 
            _factory;

        public CreateSessionPageTests(
            CustomWebApplicationFactory<ScrumPokerPlanning.Startup> factory)
        {
            _factory = factory;

            _client = Utilities.ReturnAuthorized(_factory);

            //_client = factory.CreateClient(new WebApplicationFactoryClientOptions
            //    {
            //        AllowAutoRedirect = false
            //    });
        }
        #endregion

        #region snippet2
        [Fact(Skip = "Not Ready Yet")]
        public async Task Post_DeleteAllMessagesHandler_ReturnsRedirectToRoot()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='messages']"),
                (IHtmlButtonElement)content.QuerySelector("button[id='deleteAllBtn']"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
        }
        #endregion

        #region snippet3
        [Fact(Skip = "Not Ready Yet")]
        public async Task Post_DeleteMessageHandler_ReturnsRedirectToRoot()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var serviceProvider = services.BuildServiceProvider();

                        using (var scope = serviceProvider.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices
                                .GetRequiredService<ApplicationContext>();
                            var logger = scopedServices
                                .GetRequiredService<ILogger<CreateSessionPageTests>>();

                            try
                            {
                                Utilities.ReinitializeDbForTests(db);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "An error occurred seeding " +
                                    "the database with test messages. Error: {Message}", 
                                    ex.Message);
                            }
                        }
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
            var defaultPage = await client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //Act
            var response = await client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='messages']"),
                (IHtmlButtonElement)content.QuerySelector("form[id='messages']")
                    .QuerySelector("div[class='panel-body']")
                    .QuerySelector("button"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
        }
        #endregion

        [Fact(Skip = "Not Ready Yet")]
        public async Task Post_AddMessageHandler_ReturnsSuccess_WhenMissingMessageText()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var messageText = string.Empty;

            // Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='account']"),
                (IHtmlButtonElement)content.QuerySelector("button[id='logInButtonID']"),
                new Dictionary<string, string> {
                    ["Input.Email"] = "Sanchessls@gmail.com",
                    ["Input.Password"] = "ASLS12asls12@"
                });

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            // A ModelState failure returns to Page (200-OK) and doesn't redirect.
            response.EnsureSuccessStatusCode();
            Assert.Null(response.Headers.Location?.OriginalString);
        }

        [Fact(Skip = "Not Ready Yet")]
        public async Task Post_AddMessageHandler_ReturnsSuccess_WhenMessageTextTooLong()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
            var messageText = new string('X', 201);

            // Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='addMessage']"),
                (IHtmlButtonElement)content.QuerySelector("button[id='addMessageBtn']"),
                new Dictionary<string, string> {
                    ["Message.Text"] = messageText
                });

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            // A ModelState failure returns to Page (200-OK) and doesn't redirect.
            response.EnsureSuccessStatusCode();
            Assert.Null(response.Headers.Location?.OriginalString);
        }

        [Fact(Skip = "Not Ready Yet")]
        public async Task Post_AnalyzeMessagesHandler_ReturnsRedirectToRoot()
        {
            // Arrange
            var defaultPage = await _client.GetAsync("/Identity/planning/create");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            //string result = defaultPage.Content.ReadAsStringAsync().Result;


            //Act
            var response = await _client.SendAsync(
                (IHtmlFormElement)content.QuerySelector("form[id='formCreateSession']"),
                (IHtmlInputElement)content.QuerySelector("input[id='idButtonCreateSession']"));
            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);
            //Assert.Equal(result, "");
        }

    }
}
