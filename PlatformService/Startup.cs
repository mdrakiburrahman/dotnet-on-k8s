using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Grpc;
using PlatformService.SyncDataServices.Http;

namespace PlatformService
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        // Added in variable to know which environment we are in
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using SQL Server");
                services.AddDbContext<AppDbContext>(opt => 
                    opt.UseSqlServer(Configuration.GetConnectionString("PlatformsConn")));
            }
            else
            {
                Console.WriteLine("--> Using InMem DB");
                // Starts up with an In Memory Database
                services.AddDbContext<AppDbContext>(opt => 
                    opt.UseInMemoryDatabase("InMem"));
            }
                
            // If someone asks for IPlatformRepo, give them PlatformRepo
            // This is a standard pattern for dependency injection
            // We register an Interface and then the Concrete Implementation of it
            services.AddScoped<IPlatformRepo, PlatformRepo>();

            // Add Automapper to map DTOs to our Internal Models
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add our Controllers
            services.AddControllers();

            // Map from Interface to Concrete Implementation - aka this is injected via HTTP Client Factory from Startup.cs
            services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

            // Map from RabbitMQ Interface to Class
            services.AddSingleton<IMessageBusClient, MessageBusClient>();

            // Add gRPC
            services.AddGrpc();

            // - - - - - - - - - - - - - - - - - - - - - - - -
            // More later for SQL Server
            // - - - - - - - - - - - - - - - - - - - - - - - -

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlatformService", Version = "v1" });
            });

            Console.WriteLine($"--> Command Service Endpoint {Configuration["CommandService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlatformService v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Map the endpoints for the servers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                
                // gRPC Service we created
                endpoints.MapGrpcService<GrpcPlatformService>();

                // Serve up the proto filew
                endpoints.MapGet("/Protos/platforms.proto", async context =>
                {
                    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
                });
            });

            // - - - - - - - - - - - - - - - - - - - - - - - -
            // Prepare our Mockup Database
            // - - - - - - - - - - - - - - - - - - - - - - - -
            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}
