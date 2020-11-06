using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CarShop.Core.ApplicationService.Services;
using CarShop.Core.ApplicationService.Implemination;
using CarShop.Core.DomainService;
using CarShop.Infrastructure.Data.Repositories;
using CarShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CarShopWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsDevelopment())
            {
                services.AddDbContext<CarAppDBContext>(
                    opt =>
                    {
                        opt.UseSqlite("Data Source=carapp.db");
                    });
            }
            else {
                services.AddDbContext<CarAppDBContext>
                    (opt =>
                    {
                    opt.UseSqlServer(Configuration.GetConnectionString("defaultConnection"));
                });
            }

            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarRepository, CarSQLRepository>();
            #region CORS
            services.AddCors(options => options.AddPolicy("AllowEverything", builder => builder.AllowAnyOrigin()
                                                                                               .AllowAnyMethod()
                                                                                               .AllowAnyHeader()));
            #endregion
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {
                using var scope = app.ApplicationServices.CreateScope();
                var ctx = scope.ServiceProvider.GetRequiredService<CarAppDBContext>();
                var dataInitializer = scope.ServiceProvider.GetRequiredService<IDataInitializer>();


                ctx.Database.EnsureCreated();

                ctx.Database.ExecuteSqlRaw("DROP TABLE Cars");

                ctx.Database.EnsureCreated();
                dataInitializer.InitData();
                //using (var scope = app.ApplicationServices.CreateScope())
                //{

                //    var ctx = scope.ServiceProvider.GetService<CarAppDBContext>();
                //    ctx.Database.EnsureDeleted();
                //    ctx.Database.EnsureCreated();

                //    var carRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();

                //    var dataInitializer = new DataInitializer(carRepository);

                //    dataInitializer.InitData();
                //}
            }
                app.UseCors("AllowEverything");

                app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthorization();

                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            
        }
    }
}
