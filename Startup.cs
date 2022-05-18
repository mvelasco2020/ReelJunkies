using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReelJunkies.Data;
using ReelJunkies.Models.Settings;
using ReelJunkies.Services;
using ReelJunkies.Services.Interfaces;

namespace ReelJunkies
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
            //
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Services.ConnectionService.GetConnectionString(Configuration)));

            services.AddDatabaseDeveloperPageExceptionFilter();

            // services.AddDefaultIdentity<IdentityUser>(options =>
            //                                       options.SignIn.RequireConfirmedAccount = true)
            // .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                                                            options.SignIn.RequireConfirmedEmail = false)
                    .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();

            services.AddControllersWithViews();

            //register and inject a configured appsettings as stongly typed object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //this is needed bec TmDbMovieService uses and injected instance of http client
            services.AddHttpClient();

            //register movieservice
            services.AddScoped<IRemoteMovieService, TmDbMovieService>();

            //register datamapping service
            services.AddScoped<IDataMappingService, TmDbMappingService>();

            //register ImageService
            services.AddSingleton<IImageService, ImageService>();

            //register seed service
            services.AddTransient<SeedService>();

            //register mail service
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IAppEmailSender, AppEmailSender>();

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
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
            });
        }
    }
}
