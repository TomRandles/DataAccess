using DataAccessPatterns.UnitOfWorkPattern;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RepositoryPattern.Repositories;
using Shared.DataAccess;
using Shared.Domain.Models;
using System;

namespace ShoppingApp.Web
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

            services.AddControllersWithViews();

            // services.AddTransient<ShoppingDb>();
            services.AddDbContext<ShoppingDb>(options => options.UseSqlServer(@"Data Source=(localdb)\mssqllocaldb;Initial Catalog=ShoppingApp.ShoppingDb;Integrated Security=True"));

            var serviceProvider = services.BuildServiceProvider();
                var context = serviceProvider
                                      .GetService<ShoppingDb>();

            CreateInitialDatabase(context);

            //services.TryAddTransient<IRepository<Customer>, CustomerRepository>();
            //services.TryAddTransient<IRepository<Product>, ProductRepository>();
            //services.TryAddTransient<IRepository<Order>, OrderRepository >();

            services.TryAddTransient<IUnitOfWork, UnitOfWork>();
            
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Order}/{action=Index}/{id?}");
            });
        }

        public void CreateInitialDatabase(ShoppingDb context)
        {

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var camera = new Product { Name = "Canon EOS 70D", Price = 599m };
            var microphone = new Product { Name = "Shure SM7B", Price = 245m };
            var light = new Product { Name = "Key Light", Price = 59.99m };
            var phone = new Product { Name = "Android Phone", Price = 259.59m };
            var speakers = new Product { Name = "5.1 Speaker System", Price = 799.99m };

            context.Products.Add(camera);
            context.Products.Add(microphone);
            context.Products.Add(light);
            context.Products.Add(phone);
            context.Products.Add(speakers);

            context.SaveChanges();
        }
    }
}
