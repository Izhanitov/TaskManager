using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using TaskManager.DAL.EF;
using TaskManager.DAL.Repositories;
using TaskManager.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskManager.BLL.Interfaces;
using TaskManager.BLL.Services;

namespace TaskManager
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {            
            services.AddCors();
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TaskManagerContext>(options => options.UseSqlServer(connection));
            services.AddControllersWithViews().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions
                 .ReferenceHandler = ReferenceHandler.Preserve;
            });
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<ITransferService, TransferService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin()
                                          .AllowAnyMethod()
                                          .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                endpoints.MapControllerRoute(
                   name: "gettasks",
                   pattern: "{controller=Home}/{action=GetTasksList}");
                endpoints.MapControllerRoute(
                   name: "gettasktreebyid",
                   pattern: "{controller=Home}/{action=GetTaskTreeById}/{id}");
                endpoints.MapControllerRoute(
                   name: "sendtask",
                   pattern: "{controller=Home}/{action=SendTask}/");
                endpoints.MapControllerRoute(
                   name: "deletetask",
                   pattern: "{controller=Home}/{action=DeleteTask}/");
                endpoints.MapControllerRoute(
                   name: "updatetask",
                   pattern: "{controller=Home}/{action=UpdateTask}/");
                endpoints.MapControllerRoute(
                   name: "gettaskbyid",
                   pattern: "{controller=Home}/{action=GetTaskById}/{id}");
                endpoints.MapControllerRoute(
                   name: "updatetaskstatus",
                   pattern: "{controller=Home}/{action=UpdateTaskStatus}/");
            });
        }
    }
}
