using FreeCourse.Services.Catalog.Configurations;
using FreeCourse.Services.Catalog.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog
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

            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri($"rabbitmq://{Configuration["RabbitMQUrl"]}:55225"), host =>
                    {
                        host.Username("guest");
                        host.Password("guest");
                    });
                });
            });
            services.AddMassTransitHostedService();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["IdentityServerURL"];
                options.Audience = "resource_catalog";
                options.RequireHttpsMetadata = false;
            });

            services.AddScoped<ICategoryService,CategoryService>(); //ICategoryService ile kar��la�t���nda categoryservice nesnesi �ret.
            services.AddScoped<ICourseService, CourseService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });


            //Art�k herhangi bir class�n ctor'unda //Datalar� appsettings /dbsettings alan�ndan okur kod a�a��da
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            //herhnagi bir class�n ctrounda bu interface ile kar��la�t���nda git Ioptions �zerinden dolu bir database settings nesnesi olu�tur o da appsettingsdeki database settings alan�na kar��l�k geliyor bu �ekilde �options �zerinden conn string db name vb de�erleri  ve tablolar� ge�mi� olaca��z
            services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value; //get requeired service ilgili servisi bulamazsa hata f�rlat�r
            });



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FreeCourse.Services.Catalog", Version = "v1" });
            });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FreeCourse.Services.Catalog v1"));
            }

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
