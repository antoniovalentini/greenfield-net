using Microsoft.Extensions.DependencyInjection;

namespace Royale.Sdk.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRoyaleSdk(this IServiceCollection services)
        {
            // TODO: check whether IMemoryCache is already in the collection
            services.AddSingleton<IRoyaleClient, RoyaleClient>();
            return services;
        }
    }
}
