using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Xamarin.Forms;

namespace Avalentini.Expensi.Mobile.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationService _fallback;
        private readonly HttpClient _client;
        private DiscoveryDocumentResponse _disco;

        public IdentityCookie Cookie { get; set; } = new IdentityCookie("", DateTime.MinValue);

        public AuthenticationService(IAuthenticationService fallback)
        {
            _fallback = fallback;
            _client = new HttpClient();
        }

        private async Task<bool> Init(CancellationToken cancellationToken)
        {
            _disco = await _client.GetDiscoveryDocumentAsync("http://localhost:49452", cancellationToken);
            if (_disco == null || _disco.IsError)
                return false;

            //if (Cookie.IsValid())
            //    _client.SetBearerToken(Cookie.Value);

            return true;
        }

        //public async Task<bool> LoginAsync(string username, string password)
        //{
        //    return await Task.FromResult(username == "anto" && password == "test");
        //}
        public async Task<bool> TrySilentLoginAsync(CancellationToken cancellationToken)
        {
            if (!Cookie.IsValid()) return await Task.FromResult(false);
            _client.SetBearerToken(Cookie.Value);
            return await Task.FromResult(true);
        }

        public async Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken)
        {
            if (_disco == null || _disco.IsError)
            {
                using (var source = new CancellationTokenSource(TimeSpan.FromSeconds(15)))
                {
                    var isInit = await Init(source.Token);
                    if (!isInit)
                        return await _fallback.LoginAsync(username, password, default);
                }
            }
            var response = await _client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = _disco.TokenEndpoint,
                ClientId = "ro.client",
                ClientSecret = "secret",

                UserName = username, // bob
                Password = password, // password
                Scope = "api1",
                GrantType = OidcConstants.GrantTypes.Password,
            }, cancellationToken);

            if (response.IsError)
                return false;

            Cookie.Refresh(response.AccessToken, DateTime.Now.AddSeconds(response.ExpiresIn));
            _client.SetBearerToken(response.AccessToken);

            return true;
        }
    }

    //public abstract class BaseAuthenticationService : IAuthenticationService
    //{
    //    private readonly IAuthenticationService _fallback;
    //    private bool _offline;

    //    protected BaseAuthenticationService(IAuthenticationService fallback)
    //    {
    //        _fallback = fallback;
    //    }

    //    public Task<bool> Init(CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<bool> LoginAsync(string username, string password, CancellationToken cancellationToken)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public abstract Task<bool> DoLoginAsync(string username, string password, CancellationToken cancellationToken);

    //}

    public class IdentityCookie
    {
        private const string IdentityCookieKey = "identity_cookie";
        public DateTime Expiration { get; private set; }
        public string Value { 
            get =>
                Application.Current.Properties.ContainsKey (IdentityCookieKey) 
                    ? Application.Current.Properties[IdentityCookieKey].ToString () 
                    : "";
            set
            {
                Application.Current.Properties[IdentityCookieKey] = value;
                Application.Current.SavePropertiesAsync ();
            }
        }

        public IdentityCookie(string identityToken, DateTime expiration)
        {
            Value = identityToken;
            Expiration = expiration;
        }

        public void Refresh(string identityToken, DateTime expiration)
        {
            Value = identityToken;
            Expiration = expiration;
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Value) && Expiration > DateTime.Now;
        }
    }
}
