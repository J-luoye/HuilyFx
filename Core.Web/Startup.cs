using System;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Core.Web.Services;
using DataLib;
using DataLib.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Core.Web
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
            services.AddDbContext<SqlServerDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),b=> { b.MigrationsAssembly("Core.Web"); });
            });

            //services.AddRazorPages();

            services.AddMvc();
            services.AddSignalR();
            services.AddControllersWithViews();

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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Index}/{action=Index}/{id?}");


                endpoints.MapHub<ChatHouService>("/hub");
            });


        }

        //public static IServiceProvider AddLuna<TModule>(this IServiceCollection services) where TModule : IModule, new()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.Populate(services);
        //    builder.RegisterModule<TModule>();

        //    return new AutofacServiceProvider(builder.Build());
        //}

        //public class AutofacModule : Module
        //{
        //    protected override void Load(ContainerBuilder builder)
        //    {
        //        builder.RegisterType<TestContext>();

        //        builder.RegisterGeneric(typeof(TestRepository<,>)).As(typeof(IRepository<,>))
        //            .InstancePerLifetimeScope();
        //    }
        //}
    }
}
