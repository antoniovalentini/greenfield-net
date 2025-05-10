using System.Collections.Generic;
using Duende.IdentityServer.Models;

namespace Avalentini.Expensi.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };
        }

        /// <summary>
        /// An API is a resource in your system that you want to protect.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("api1", "Expensi API")
            };
        }

        /// <summary>
        /// define clients that can access this API.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients(string clientSecret, string roClientSecret)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret(clientSecret.Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" },
                },

                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret(roClientSecret.Sha256())
                    },
                    AllowedScopes = { "api1" },
                }
            };
        }
    }
}
