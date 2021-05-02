using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Avalentini.Expensi.Web.Data
{
    public interface IApiWrapper
    {
        Task<HttpResponseMessage> GetAll();
        Task<HttpResponseMessage> GetById(string id);
        Task<HttpResponseMessage> PostExpense(StringContent content);
        Task<HttpResponseMessage> PutExpense(string id, StringContent content);
        Task<HttpResponseMessage> DeleteExpense(string id);
        bool HasIdentity();
        string GetIdentity();
        Task<bool> LoginWithUsernameAndPassword(string username, string password);
        Task Logout();
        Task RegisterUser(Api.Contracts.Models.User user);
    }

    public class ApiWrapper : IApiWrapper
    {
        private readonly string _clientSecret;
        private readonly string _roClientSecret;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;
        private string _accessToken;
        private readonly DiscoveryDocumentResponse _disco;
        private const string IdentityCookie = "identity_cookie";

        public ApiWrapper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _clientSecret = configuration["clientSecret"];
            _roClientSecret = configuration["roClientSecret"];
            _httpContextAccessor = httpContextAccessor;
            _client = new HttpClient();
            _disco = Discovery().GetAwaiter().GetResult();

            if (!HasIdentity()) return;
            var cookie = _httpContextAccessor.HttpContext.Request.Cookies[IdentityCookie];
            _accessToken = cookie;
            _client.SetBearerToken(cookie);
        }

        private async Task<DiscoveryDocumentResponse> Discovery()
        {
            return await _client.GetDiscoveryDocumentAsync("http://localhost:49452");
        }

        public async Task RegisterUser(Api.Contracts.Models.User user)
        {
            try
            {
                // fetch access token
                var response = await _client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = _disco.TokenEndpoint,
                    ClientId = "client",
                    ClientSecret = _clientSecret,
                    Scope = "api1"
                });
                if (response.IsError)
                    throw new Exception("Error during RequestClientCredentialsTokenAsync." +
                                        $"Error: {response.Error}. Error description: {response.ErrorDescription}.");
                _client.SetBearerToken(response.AccessToken);

                // use the access_token to post a new user
                var content = new StringContent(JsonConvert.SerializeObject(user), 
                    Encoding.Default, "application/json");
                var userResponse = await _client.PostAsync("http://localhost:52504/api/users", content);
                if (!userResponse.IsSuccessStatusCode)
                    throw new Exception("Unable to register the user: " +
                                        $"StatusCode: {userResponse.StatusCode}");
            }
            catch
            {
                _client.SetBearerToken("");
                throw;
            }

            // login with the user credentials
            var success = await LoginWithUsernameAndPassword(user.Username, user.Password);

            if (!success)
                throw new Exception("Unable to login with registered user credentials.");
        }

        public async Task<bool> LoginWithUsernameAndPassword(string username, string password)
        {
            var response = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = _disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = _roClientSecret,

                UserName = username, // bob
                Password = password, // password
                Scope = "api1",
                GrantType = OidcConstants.GrantTypes.Password,
            });

            if (response.IsError)
                return false;

            _accessToken = response.AccessToken;
            StoreIdentity(response.AccessToken, DateTime.Now.AddSeconds(response.ExpiresIn));
            _client.SetBearerToken(response.AccessToken);

            return true;
        }

        public async Task Logout()
        {
            var response = await _client.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = _disco.RevocationEndpoint,
                ClientId = "ro.client",
                ClientSecret = _roClientSecret,
                Token = _accessToken,
            });

            if (response.IsError)
                throw new Exception($"Error during logout: {response.Error}");

            _client.SetBearerToken("");
            DeleteIdentity();
        }

        public bool HasIdentity()
        {
            return _httpContextAccessor.HttpContext.Request.Cookies[IdentityCookie] != null;
        }

        private void StoreIdentity(string accessToken, DateTime expiration)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Append(IdentityCookie, accessToken,
                new CookieOptions
                {
                    Expires = expiration
                });
        }

        private void DeleteIdentity()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies[IdentityCookie] != null)
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(IdentityCookie);
        }

        private void CheckConnection()
        {
            if (!HasIdentity())
                throw new Exception("No identity found in cookies!");
        }

        public async Task<HttpResponseMessage> GetAll()
        {
            CheckConnection();
            return await _client.GetAsync("http://localhost:52504/api/expenses");
        }

        public async Task<HttpResponseMessage> GetById(string id)
        {
            CheckConnection();
            return await _client.GetAsync($"http://localhost:52504/api/expenses/{id}");
        }

        public async Task<HttpResponseMessage> PostExpense(StringContent content)
        {
            CheckConnection();
            return await _client.PostAsync("http://localhost:52504/api/expenses", content);
        }

        public async Task<HttpResponseMessage> PutExpense(string id, StringContent content)
        {
            CheckConnection();
            return await _client.PutAsync($"http://localhost:52504/api/expenses/{id}", content);
        }

        public async Task<HttpResponseMessage> DeleteExpense(string id)
        {
            CheckConnection();
            return await _client.DeleteAsync($"http://localhost:52504/api/expenses/{id}");
        }

        public string GetIdentity()
        {
            CheckConnection();
            return _accessToken;
        }
    }
}
