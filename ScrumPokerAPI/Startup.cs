using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Repositories.Implementation;
using ScrumPokerAPI.Repositories.Interface;
using System.Text;

namespace ScrumPokerAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Here we are adding to the Entity Framework our Db Context with our connection string 
            services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ScrumPokerConnection")));

            //Adding the identity for autorization and autenticaton
            services.AddIdentity<IdentityUser, IdentityRole>(options => { options.Password.RequireDigit = true;
                 options.Password.RequiredLength = 6;
                                                                        
                }).AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders(); // Configure token generator

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(OptionsBuilderConfigurationExtensions => {

                OptionsBuilderConfigurationExtensions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "website",
                    ValidIssuer = "website",
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Key used to encryption")),
                    ValidateIssuerSigningKey = true
                };
            });

            //Add all of the controllers 
            services.AddControllers();

            //Adding the repository to an interface as Scooped (One instance per request)
            //this is used in our Dependence injection
            //Everytime that the system ask for the first param, the system will serve an instance of the second one
            //This allow us to change the repository without haveing to change the original one.

            services.AddScoped<IRepositoryTableOne, RepositoryTableOneImp>();
            services.AddScoped<IRepositoryTableTwo, RepositoryTableTwoImp>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
