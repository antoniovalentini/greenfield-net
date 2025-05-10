using System.Security.Claims;
using AutoMapper;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data;
using Avalentini.Expensi.Api.Extensions;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Avalentini.Expensi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ILog Logger { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            var connString = Configuration["MongoDbConnection"];
            services.AddMongoDbCollection<ExpensesPerUser>(connString,
                "expensi", "expenses");

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<ExpenseMongoEntity, Expense>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom( entity => entity.ExpenseId));
                cfg.CreateMap<Expense, ExpenseMongoEntity>()
                    .ForMember(dest => dest.CreationDate, opt => opt.Ignore())
                    .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.Id));
                cfg.CreateMap<ExpenseEntity, Expense>();
                cfg.CreateMap<Expense, ExpenseEntity>()
                    .ForMember(dest => dest.CreationDate, opt => opt.Ignore());
                cfg.CreateMap<UserEntity, User>();
                cfg.CreateMap<User, UserEntity>()
                    .ForMember(dest => dest.CreationDate, opt => opt.Ignore());
            }, typeof(Startup));
            // Mapper.AssertConfigurationIsValid(); // assert should be called to the DI injected instance and not the static one

            Logger = services.AddLog4Net();

            services.AddCors();
            services.AddMvcCore()
                .AddAuthorization(options =>
                {
                    // requires the NameIdentifier claims to be present
                    // in order to be considered as a valid user
                    options.AddPolicy("mustBeUser", builder => { builder.RequireClaim(ClaimTypes.NameIdentifier); });
                })
                .AddNewtonsoftJson()
                .AddApiExplorer();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "http://localhost:49452";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1";
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Expensi API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.ConfigureExceptionHandler(Logger);

            //TODO: remember to fix this in release
            app.UseCors(builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expensi API V1");
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
