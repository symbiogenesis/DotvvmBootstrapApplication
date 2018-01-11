using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RingDownConsole.Interfaces;
using RingDownConsole.Models;
using RingDownConsole.Models.Utils;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace RingDownConsole.WebAPI
{
    public class Startup
    {
        private string _clientUri;

        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            services.AddDataProtection();
            services.AddAuthorization();
            services.AddWebEncoders();

            services.Configure<AppSettings>(Configuration);

            services.AddMvc();
            
            services.AddDbContext<RingDownConsoleDbContext>(opt =>
            {
                var connectionString = Configuration.GetValue<string>(nameof(AppSettings.RingDownConsoleDb));
                opt.UseSqlServer(connectionString, o => o.UseRowNumberForPaging());
                opt.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            //services.AddDbContext<ProcurementDbContext>(opt => opt.UseInMemoryDatabase("ProcurementDb"));

            services.AddIdentity<User, Role>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 0;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireLowercase = false;
                config.Password.RequireUppercase = false;
                config.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<RingDownConsoleDbContext>()
            .AddDefaultTokenProviders();


            services.AddAuthentication();

            services.AddScoped<IDbInitializer, DbInitializer>();

            _clientUri = Configuration.GetValue<string>("ClientUri");

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder => builder.WithOrigins(_clientUri).AllowAnyHeader().AllowAnyMethod());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Procurement Tracking Web API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IDbInitializer dbInitializer)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#if DEBUG
            dbInitializer.Initialize().Wait();
#endif

            app.UseCors("AllowSpecificOrigin");

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //serviceScope.ServiceProvider.GetService<AirfieldTrainingContext>().Database.EnsureCreated();
                var escortLogDbContext = serviceScope.ServiceProvider.GetService<RingDownConsoleDbContext>();

                escortLogDbContext.Database.EnsureCreated();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Procurement Tracking Web V1");
            });

            app.UseMvc();
        }
    }
}
