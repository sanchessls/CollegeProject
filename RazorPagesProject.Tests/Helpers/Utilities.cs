using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;

namespace RazorPagesProject.Tests
{
    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(ApplicationContext db)
        {
            db.Users.AddRange(GetUsers());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationContext db)
        {
            db.Users.RemoveRange(db.Users);
            InitializeDbForTests(db);
        }

        public static List<ApplicationUser> GetUsers()
        {
            return new List<ApplicationUser>()
            {
                //new IdentityUser(){ Id = "id1" , UserName = "Test user",Email = "email@eemail.com" ,EmailConfirmed = true,}
            };
        }

        public static HttpClient ReturnAuthorized(CustomWebApplicationFactory<ScrumPokerPlanning.Startup> _factory)
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            })
   .CreateClient(new WebApplicationFactoryClientOptions
   {
       AllowAutoRedirect = true,
   });

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");

            return client;
        }
        #endregion
    }
}
