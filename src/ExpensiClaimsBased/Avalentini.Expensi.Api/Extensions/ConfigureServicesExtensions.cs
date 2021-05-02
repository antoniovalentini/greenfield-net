using System.IO;
using System.Net;
using System.Reflection;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Domain.Data.General;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Avalentini.Expensi.Api.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static ILog AddLog4Net(this IServiceCollection services)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            var logger = LogManager.GetLogger(typeof(Program));
            services.AddSingleton(logger);
            return logger;
        }

        public static void AddMongoDbCollection<T>(this IServiceCollection services, string mongoConnectionString, string mongoDbName, string mongoCollectionName)
        {
            var client = new MongoClient(mongoConnectionString);
            var database = client.GetDatabase(mongoDbName);
            var collection = database.GetCollection<T>(mongoCollectionName);
            services.AddSingleton(collection);
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILog logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
 
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    { 
                        logger.Error($"Something went wrong: {contextFeature.Error.Message}", contextFeature.Error);
 
                        var error = JsonConvert.SerializeObject(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error."
                        });
                        await context.Response.WriteAsync(error);
                    }
                });
            });
        }
    }
}
