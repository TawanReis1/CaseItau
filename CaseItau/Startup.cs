using System;
using System.Reflection;
using API.Mapping;
using AutoMapper;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Services;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace API
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
            services.AddControllers()
                .AddNewtonsoftJson();

            // Injeção de dependência para Informação Campeonato
            services.AddScoped<IInformacaoCampeonatoService, InformacaoCampeonatoService>();

            Assembly[] autoMapper = new Assembly[] { typeof(_EntityBaseProfile).GetTypeInfo().Assembly, Assembly.GetExecutingAssembly() };
            services.AddAutoMapper(autoMapper);

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Informações Campeonato API",
                    Description = "API para teste Itaú. Análise do Campeonato Brasileiro, período de 2015 a 2019.",
                    Contact = new OpenApiContact() { Name = "Tawan Reis de Abreu", Email = "tawanreis1@hotmail.com", Url = new Uri("https://github.com/TawanReis1") }
                });

                c.EnableAnnotations();

                c.CustomSchemaIds(i => i.FullName);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string routePrefix = string.Empty;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                routePrefix = "informacoes-campeonato/";
                app.UseStaticFiles("/informacoes-campeonato");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwagger(c =>
            {
                if (!env.IsDevelopment())
                {
                    c.RouteTemplate = routePrefix + "swagger/{documentname}/swagger.json";
                }
            });

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = routePrefix + "docs";
                c.SwaggerEndpoint($"/{routePrefix}swagger/v1/swagger.json", "Informações-Campeonato V" + "1");
                c.InjectStylesheet($"/{routePrefix}swagger-ui/custom.css");
                c.DocExpansion(DocExpansion.None);
                c.ConfigObject.DisplayOperationId = false;
            });

        }
    }
}
