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

            services.AddScoped<ICategoryService,CategoryService>(); //ICategoryService ile karþýlaþtýðýnda categoryservice nesnesi üret.
            services.AddScoped<ICourseService, CourseService>();

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            });


            //Artýk herhangi bir classýn ctor'unda //Datalarý appsettings /dbsettings alanýndan okur kod aþaðýda
            services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
            //herhnagi bir classýn ctrounda bu interface ile karþýlaþtýðýnda git Ioptions üzerinden dolu bir database settings nesnesi oluþtur o da appsettingsdeki database settings alanýna karþýlýk geliyor bu þekilde ýoptions üzerinden conn string db name vb deðerleri  ve tablolarý geçmiþ olacaðýz
            services.AddSingleton<IDatabaseSettings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value; //get requeired service ilgili servisi bulamazsa hata fýrlatýr
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
