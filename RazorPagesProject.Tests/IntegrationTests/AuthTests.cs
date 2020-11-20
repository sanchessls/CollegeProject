using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AngleSharp.Html.Dom;
using Xunit;
using RazorPagesProject.Tests.Helpers;
using System;

namespace RazorPagesProject.Tests
{
    public class AuthTests : 
        IClassFixture<CustomWebApplicationFactory<ScrumPokerPlanning.Startup>>
    {
        private readonly CustomWebApplicationFactory<ScrumPokerPlanning.Startup> 
            _factory;

        public AuthTests(
            CustomWebApplicationFactory<ScrumPokerPlanning.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser()
        {
            await TestUnauthorizedPageAsync("/Identity/Planning/Create");
            await TestUnauthorizedPageAsync("/");
        }

        [Fact(Skip ="Not working authorization yet")]
        public async Task Get_SecurePageRedirectsAnAuthenticatedUser()
        {
            await TestAuthorizedPageAsync("/Identity/Planning/Create");
            await TestAuthorizedPageAsync("/");
        }

        private async Task TestUnauthorizedPageAsync(string page)
        {
            // Arrange
            var client = Utilities.ReturnUnauthorized(_factory);

            // Act
            var response = await client.GetAsync(page);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/Identity/Account/Login", response.Headers.Location.AbsolutePath);
        }

        private async Task TestAuthorizedPageAsync(string page)
        {
            // Arrange
            var client = Utilities.ReturnAuthorized(_factory);

            // Act
            var response = await client.GetAsync(page);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            //Assert.Contains("Identity/Account/Login", response.RequestMessage.RequestUri.ToString());
        }

    }

    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
