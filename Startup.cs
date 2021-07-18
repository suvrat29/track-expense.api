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
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            //enable in-memory cache
            services.AddMemoryCache();
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                        .GetBytes(Configuration["AuthSettings:Token"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddDbContext<PostgreSQLContext>(options => options.UseNpgsql(Configuration["ConnectionString"]));

            //Add table interfaces here
            services.AddScoped<IUserModelProvider, UserModelProvider>();
            services.AddScoped<IApplicationlogProvider, ApplicationlogProvider>();
            //Add service interfaces here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApplogService, ApplogService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMemCacheService, MemCacheService>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}