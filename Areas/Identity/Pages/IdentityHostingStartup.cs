using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;

[assembly: HostingStartup(typeof(ScrumPokerPlanning.Areas.Identity.IdentityHostingStartup))]
namespace ScrumPokerPlanning.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                //Adding the identity for autorization and autenticaton
                //    services.AddIdentity<IdentityUser, IdentityRole>(options => { /*options.SignIn.RequireConfirmedAccount*/
                //        options.Password.RequireDigit = true;
                //        options.Password.RequiredLength = 6;
                //    }).AddEntityFrameworkStores<ApplicationContext>()
                //         .AddDefaultUI()
                //        .AddDefaultTokenProviders(); // Configure token generator

                


            });



        }
    }
}