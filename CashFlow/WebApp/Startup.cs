using CashFlow.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Helpers;

namespace WebApp
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            HostingEnvironment = environment;
            Configuration = configuration;
        }
        public IWebHostEnvironment HostingEnvironment { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string nameMachineName = Environment.MachineName;
            int DataBase = Configuration.GetSection("ApplicationConfig").GetValue<int>("DataBase");

            if (HostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(nameMachineName))
            {
                services.AddEntityFrameworkMySql().AddDbContext<CashFlowContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL"), ServerVersion.AutoDetect(Configuration.GetConnectionString("MySQL"))));
            }
            else
            {
                services.AddEntityFrameworkMySql().AddDbContext<CashFlowContext>(options => options.UseMySql(Configuration.GetConnectionString("MySQL"), ServerVersion.AutoDetect(Configuration.GetConnectionString("MySQL"))));
            }

            services.Configure<ApplicationConfig>(Configuration.GetSection("ApplicationConfig"));

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<CashFlowContext>();
        }

        protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string nameMachineName = Environment.MachineName;
            

            if (HostingEnvironment.IsDevelopment() && !string.IsNullOrEmpty(nameMachineName))
            {
                ServerVersion serverVersion = ServerVersion.AutoDetect(Configuration.GetConnectionString(nameMachineName.ToUpper()));
                optionsBuilder.UseMySql(Configuration.GetConnectionString(nameMachineName.ToUpper()), serverVersion);
                Console.WriteLine("IsDevelopment");
            }
            else
            {
                ServerVersion serverVersion = ServerVersion.AutoDetect(Configuration.GetConnectionString("DefaultConnection"));
                optionsBuilder.UseMySql(Configuration.GetConnectionString("DefaultConnection"), serverVersion);
                Console.WriteLine("Default");
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            LogLevel level = Configuration.GetSection("Logging:LogLevel").GetValue<LogLevel>("Default");

            SetLogger(level);

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            RunMigrationUpdateDatabase(app);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        private void SetLogger(LogLevel level)
        {
            if (HostingEnvironment.IsDevelopment())
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.LiterateConsole()
                    .WriteTo.RollingFile(@"Logs/CashFlow.log", retainedFileCountLimit: 7)
                    .WriteTo.Seq("http://localhost:5341")
                    .CreateLogger();
            }
            else
            {
                if (level.Equals(LogLevel.Debug))
                {
                    Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.LiterateConsole()
                    .WriteTo.RollingFile(@"Logs/CashFlow.log", retainedFileCountLimit: 7)
                    .WriteTo.Seq("http://localhost:5341")
                    .CreateLogger();
                }
                else
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Error()
                        .Enrich.FromLogContext()
                        .WriteTo.LiterateConsole()
                        .WriteTo.RollingFile(@"Logs/CashFlow.log", retainedFileCountLimit: 7)
                        .WriteTo.Seq("http://localhost:5341")
                        .CreateLogger();
                }
            }
        }

        private void RunMigrationUpdateDatabase(IApplicationBuilder app)
        {
            try
            {
                bool migrationsUpdate = Configuration.GetSection("ApplicationConfig").GetValue<bool>("MigrationsDatabaseUpdate");

                if (migrationsUpdate)
                {
                    using IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
                    var contextCore = serviceScope.ServiceProvider.GetService<CashFlowContext>();
                    contextCore.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Log.Error("Error Runs the migration update database.", ex);
            }
        }
    }
}
