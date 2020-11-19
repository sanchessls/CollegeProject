using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace RazorPagesProject.Tests.IntegrationTests
{
    #region snippet1
    public class BasicTests 
        : IClassFixture<WebApplicationFactory<ScrumPokerPlanning.Startup>>
    {
        private readonly WebApplicationFactory<ScrumPokerPlanning.Startup> _factory;

        public BasicTests(WebApplicationFactory<ScrumPokerPlanning.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        //[InlineData("/")]
        [InlineData("Identity/Account/Login")]
        [InlineData("Identity/Account/Register")]
       // [InlineData("Identity/Planning/ActiveList")]
        //[InlineData("Identity/Planning/Create")]
        //[InlineData("Identity/Planning/Feature")]
        //[InlineData("Identity/Planning/Join")]
        //[InlineData("Identity/Planning/Session")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }
    }
    #endregion
}
