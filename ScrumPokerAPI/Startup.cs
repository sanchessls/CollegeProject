using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Repositories.Implementation;
using ScrumPokerAPI.Repositories.Interface;
using ScrumPokerAPI.Services;
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
            //services.AddDbContext<UserContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ScrumPokerConnection")));

            ////Adding the identity for autorization and autenticaton
            //services.AddIdentity<IdentityUser, IdentityRole>(options =>
            //{ /*options.SignIn.RequireConfirmedAccount*/
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 6;

            //}).AddEntityFrameworkStores<ApplicationContext>()
            //     .AddDefaultUI()
            //    .AddDefaultTokenProviders(); // Configure token generator


            //services.AddAuthentication(auth =>
            //{                
            //    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(OptionsBuilderConfigurationExtensions =>
            //{

            //    OptionsBuilderConfigurationExtensions.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = Configuration["AuthSettings:Audience"],
            //        ValidIssuer = Configuration["AuthSettings:Issuer"],
            //        //RequireExpirationTime = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
            //        ValidateIssuerSigningKey = true
            //    };
            //});

            //Injection dependence for the User service to manage Users
            services.AddScoped<IUserService, UserService>();

            //Add all of the controllers 
            services.AddControllers();

            //Adding the repository to an interface as Scooped (One instance per request)
            //this is used in our Dependence injection
            //Everytime that the system ask for the first param, the system will serve an instance of the second one
            //This allow us to change the repository without haveing to change the original one.

            services.AddScoped<IRepositoryPlanningSession, RepositoryPlanningSessionImp>();
            services.AddScoped<IRepositoryTableTwo, RepositoryTableTwoImp>();

            
            //Adding The Web Pages
            services.AddControllersWithViews();
            services.AddRazorPages();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Unauthorized",
                            Msg = "token expired"
                        }));
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            State = "Internal Server Error",
                            Msg = error.Error.Message
                        }));
                    }
                    //when no error, do next.
                    else await next();
                });
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
                //endpoints.MapControllers();
            });
        }
    }
}
