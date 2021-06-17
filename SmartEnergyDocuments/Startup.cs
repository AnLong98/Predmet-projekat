using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartEnergy.Contract.CommonService;
using SmartEnergy.Contract.Interfaces;
using SmartEnergy.Documents.Mapping;
using SmartEnergy.Documents.Services;
using SmartEnergy.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SmartEnergyDocuments
{
    public class Startup
    {
        private readonly string _cors = "cors";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApprovedOnly", policy => policy.RequireClaim("Approved"));
            });

            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = "http://localhost:44372",
                   ValidAudience = "http://localhost:44372",
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
               };
           });
            services.AddControllers().AddDapr()
                                     .AddJsonOptions(options =>
                                     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddDbContext<DocumentsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DocumentsDatabase")));


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Energy Documents API", Version = "v1" });

            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: _cors, builder => {
                    builder.WithOrigins("https://localhost:4200", "http://localhost:4200").AllowAnyHeader()
                                        .AllowAnyMethod().AllowCredentials();
                    /*builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials()
                           .AllowAnyOrigin();*/
                });
            });

            //Add Service implementations
            services.AddScoped<IAuthHelperService, AuthHelperService>();
            services.AddScoped<ICallService, CallService>();
            services.AddScoped<IIncidentService, IncidentService>();
            services.AddScoped<IMultimediaService, MultimediaService>();
            services.AddScoped<IResolutionService, ResolutionService>();
            services.AddScoped<ISafetyDocumentService, SafetyDocumentService>();
            services.AddScoped<IStateChangeService, StateChangeService>();
            services.AddScoped<ITimeService, TimeService>();
            services.AddScoped<IWorkRequestService, WorkRequestService>();

        }

        public void InitDb(DocumentsDbContext context)
        {
            if (!context.Exists())
            {

                context.Database.Migrate();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(_cors);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Smart Energy Documents API v1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                bool migrated = false;
                int attempts = 3;
                while (!migrated && attempts > 0)
                {


                    try
                    {
                        var context = serviceScope.ServiceProvider.GetService<DocumentsDbContext>();
                        Thread.Sleep(60000);
                        InitDb(context);
                        migrated = true;
                    }
                    catch //Catch if too soon initing
                    {
                        attempts--;

                    }
                }
            }

        }
    }
}
