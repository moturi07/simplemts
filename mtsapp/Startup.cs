using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using mtsDAL.Data;
using mtsDAL.Services;
using mtsDAL.Services.IServices;
using Serilog;
using System.Reflection;

namespace mtsapp
{
    public class Startup
    {
		private readonly ILogger _logger;
        public IConfiguration Configuration { get; }
        
        public bool IsDev { get; set; }
        public Startup(IConfiguration configuration, IHostEnvironment hostingEnvironment)
        {
            _logger = Log.ForContext<Startup>();
            Configuration = configuration;
            IsDev = hostingEnvironment.IsDevelopment();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvcCore()
                 .AddApiExplorer()
            .AddAuthorization()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            var _urls = !string.IsNullOrEmpty(Configuration["CORS"]) ? Configuration["CORS"].ToString().Split(';') : null;

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                                  builder =>
                                  builder.WithOrigins(_urls)
                                         .AllowAnyMethod()
                                         .AllowAnyHeader()
                                         .AllowCredentials());
            });
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(xmlFilename);
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Money Transfer System API",
                    Description = "API Definations for Money Transfer System"
                });
            });
            services.AddScoped<IAccountService, AccountService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("CorsPolicy");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "mtsapp v1"));
            }
            var _appSettings = configuration["CORS"];
            var origins = _appSettings.Split(";");
            app.UseCors(x => x
                      .WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowCredentials()
                      .AllowAnyHeader());

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}