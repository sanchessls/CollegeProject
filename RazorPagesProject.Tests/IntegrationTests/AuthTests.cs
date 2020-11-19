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

        #region snippet2
        [Fact(Skip = "UNTILL FIXX")]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser()
        {
            // Arrange
            var client = _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            // Act
            var response = await client.GetAsync("/");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("http://localhost/Identity/Account/Login", 
                response.Headers.Location.OriginalString);
        }
        #endregion
        
        #region snippet3        
        [Fact(Skip = "Not Ready Yet")]
        public async Task Get_SecurePageRedirectsAnUnauthenticatedUser3()
        {
            // Arrange
            var client = Utilities.ReturnAuthorized(_factory);       

            // Act
            var response = await client.GetAsync("/Identity/Planning/Join");

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.StartsWith("/Identity/Planning/Join",
                response.Headers.Location.OriginalString);
        }
        #endregion
    }
    #region snippet4
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
    #endregion
}
