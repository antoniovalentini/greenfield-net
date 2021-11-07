using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Royale.Sdk.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRoyaleSdk(
            this IServiceCollection services,
            Func<HttpMessageHandler> configureHandler = null)
        {
            // TODO: check whether IMemoryCache is already in the collection

            var clientBuilder = services.AddHttpClient<IApiClient, ApiClient>(c =>
            {
                c.BaseAddress = new Uri("https://api.clashroyale.com/v1/");
            });

            if (configureHandler is { })
                clientBuilder.ConfigurePrimaryHttpMessageHandler(configureHandler);

            services.AddSingleton<IRoyaleClient, RoyaleClient>();

            return services;
        }
    }
}
