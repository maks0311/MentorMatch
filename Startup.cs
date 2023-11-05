using Mentor.Data;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using Radzen;
using System.Linq;

namespace Mentor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Radzen Notifications
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<TooltipService>();
            services.AddScoped<ContextMenuService>();

            //Application Session Storage
            services.AddBlazoredSessionStorage();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IWorkTypeService, WorkTypeService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ICompetenceService, CompetenceService>();
            services.AddScoped<ILessonService, LessonService>();

            //SQL Server Connection
            if (Globals.AdminHostNames.Contains(System.Environment.MachineName))
            {
                SqlConnectionConfiguration sqlConnectionConfiguration = new(Configuration.GetConnectionString("Development"));
                services.AddSingleton(sqlConnectionConfiguration);
            }
            else
            {
                SqlConnectionConfiguration sqlConnectionConfiguration = new(Configuration.GetConnectionString("Production"));
                services.AddSingleton(sqlConnectionConfiguration);
            }

            services.AddRazorPages();
            services.AddServerSideBlazor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
