using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using RingDownConsole.Interfaces;
using RingDownConsole.Models;
using RingDownConsole.Models.Enums;
using RingDownConsole.Models.Utils;
using RingDownConsole.Services;

namespace RingDownConsole
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddResponseCompression(opt => opt.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" }));

            var adminPolicy = new AuthorizationPolicyBuilder(new[] { CookieAuthenticationDefaults.AuthenticationScheme })
                .RequireAuthenticatedUser()
                .RequireRole(new[] { nameof(Roles.Administrator) })
                .Build();

            var userPolicy = new AuthorizationPolicyBuilder(new[] { CookieAuthenticationDefaults.AuthenticationScheme })
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(nameof(Roles.User), userPolicy);
                options.AddPolicy(nameof(Roles.Administrator), adminPolicy);
            });

            services.AddWebEncoders();

            services.Configure<AppSettings>(Configuration);

            //services.AddDbContext<RingDownConsoleDbContext>(opt =>
            //{
            //    opt.UseInMemoryDatabase("RingDownConsoleDb");
            //    opt.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            //});

            services.AddDbContext<RingDownConsoleDbContext>(opt =>
            {
                var ringDownConsoleDb = Configuration.GetValue<string>(nameof(AppSettings.RingDownConsoleDb));
                opt.UseSqlServer(ringDownConsoleDb, o => o.UseRowNumberForPaging());
                opt.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

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

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = new PathString("/login");
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToReturnUrl = async c => await DotvvmAuthenticationHelper.ApplyRedirectResponse(c.HttpContext, c.RedirectUri),
                        OnRedirectToAccessDenied = async c => await DotvvmAuthenticationHelper.ApplyStatusCodeResponse(c.HttpContext, 403),
                        OnRedirectToLogin = async c => await DotvvmAuthenticationHelper.ApplyRedirectResponse(c.HttpContext, c.RedirectUri),
                        OnRedirectToLogout = async c => await DotvvmAuthenticationHelper.ApplyRedirectResponse(c.HttpContext, c.RedirectUri),
                        OnValidatePrincipal = async c => await ValidateAsync(c)
                    };
                });

            services
                .ConfigureApplicationCookie(options => options.LoginPath = new PathString("/login"));

            services.AddScoped<ExampleRecordService>();
            services.AddScoped<SettingsService>();
            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddDotVVM(options => options.AddDefaultTempStorages(Path.GetTempPath()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IDbInitializer dbInitializer)
        {
            loggerFactory.AddConsole();

            app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);

            app.UseAuthentication();
            app.UseResponseCompression();

#if DEBUG
            dbInitializer.Initialize().Wait();
#endif

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) => {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue()
                    {
                        MaxAge = TimeSpan.FromDays(365)
                    };
                }
            });
        }

        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();

            var user = await userManager.GetUserAsync(context.Principal);

            if (user == null)
            {
                context.ShouldRenew = true;
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
