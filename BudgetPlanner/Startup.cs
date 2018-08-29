using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetPlanner.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BudgetPlanner.Models;
using BudgetPlanner.Persistence.Interfaces;
using BudgetPlanner.Persistence;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BudgetPlanner.Persistence.Repositories;
using Autofac.Core;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BudgetPlanner
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<BudgetDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>()
                .AddEntityFrameworkStores<BudgetDbContext>();
            

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
            

            var builder = new ContainerBuilder();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<SubscriptionRepository>().As<ISubscriptionRepository>();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>();
            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.Populate(services);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSignalR(route =>
            {
                route.MapHub<BudgetHub>("/budgetHub");
            });

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "index",
                    template: "{controller=Home}/{search?}");
                routes.MapRoute(
                    name: "create",
                    template: "{controller=Home}/{action=Index}/{subId?}/{deposit?}");
            });
        }
    }
}
