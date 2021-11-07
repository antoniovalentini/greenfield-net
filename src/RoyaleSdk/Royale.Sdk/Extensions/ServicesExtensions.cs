using Microsoft.Extensions.DependencyInjection;

namespace Royale.Sdk.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRoyaleSdk(this IServiceCollection services)
        {
            services.AddSingleton<IRoyaleClient, RoyaleClient>();
            return services;
        }
    }
}
