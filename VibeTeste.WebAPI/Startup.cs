using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using VibeTeste.Application.InterfacesServices;
using VibeTeste.Application.Mapper;
using VibeTeste.Application.Services;
using VibeTeste.Application.ValidatorDTO;
using VibeTeste.Data.Repositories;
using VibeTeste.Domain.InterfaceRepositories;
using VibeTeste.Domain.ValidatorEntity;

namespace VibeTeste.WebAPI
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
            services.AddFluentValidation(validation => validation.RegisterValidatorsFromAssemblyContaining<PlacemarkValidator>());
            services.AddFluentValidation(validation => validation.RegisterValidatorsFromAssemblyContaining<PlacemarkFilterDTOValidator>());

            services.AddAutoMapper(typeof(EntityToDTO));

            var filePath = Configuration["ApiSettings:filePath"];
            services.AddSingleton<IPlacemarkRepository>(provider =>
                new PlacemarkRepository(filePath));

            services.AddSingleton<IPlacemarkService, PlacemarkService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Projeto Teste Vibe",
                    Description = "API voltada ao processo seletivo do Grupo Vibe.",
                    Contact = new OpenApiContact() { Name = "Fabricio", Email = "fabriciosilvam98@gmail.com" },
                    License = new OpenApiLicense() { Name = "License", Url = new Uri("https://linkedin.com/in/fabriciosilvam98/") },
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VibeTeste.WebAPI v1"));
            }

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
