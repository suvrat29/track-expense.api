using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using track_expense.api.DatabaseAccess;
using track_expense.api.Services.ServiceClasses;
using track_expense.api.Services.Interfaces;
using track_expense.api.TableOps.Interfaces;
using track_expense.api.TableOps.TableClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using System;

namespace track_expense.api
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
            //Build connectionstring for heroku postgres
            string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // parse the connection string
            var databaseUri = new Uri(connectionUrl);

            string db = databaseUri.LocalPath.TrimStart('/');
            string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
            /////////////////////////////////////////////////
            
            //Hangfire service
            services.AddHangfire(config => config.UsePostgreSqlStorage($"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;"));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            //enable in-memory cache
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "track_expense.api", Version = "v1" });
            });

            //TODO: Setup full auth with ValidateIssuer and ValidateAudience
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("APP_TOKEN"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<PostgreSQLContext>(options => options.UseNpgsql($"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;"));
            
            //Add table interfaces here
            services.AddScoped<IUserModelProvider, UserModelProvider>();
            services.AddScoped<IApplicationlogProvider, ApplicationlogProvider>();
            services.AddScoped<ILocalesProvider, LocalesProvider>();
            services.AddScoped<IEmaildataProvider, EmaildataProvider>();
            services.AddScoped<ICategorydataProvider, CategorydataProvider>();
            services.AddScoped<ISubcategorydataProvider, SubcategorydataProvider>();
            services.AddScoped<IUseractivitylogProvider, UseractivitylogProvider>();
            //Add service interfaces here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMemCacheService, MemCacheService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IApplogService, ApplogService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ISetupService, SetupService>();
            services.AddScoped<IUseractivitylogService, UseractivitylogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "track_expense.api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(allowed => allowed.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
