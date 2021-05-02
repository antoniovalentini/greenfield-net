using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Avalentini.Expensi.Web.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.Web.Pages.Identity
{
    public class IndexModel : PageModel
    {
        public IList<Claim> Claims { get; set; }
        public bool HasClaims => Claims != null && Claims.Count > 0;

        private readonly IApiWrapper _apiWrapper;

        public IndexModel(IApiWrapper apiWrapper)
        {
            _apiWrapper = apiWrapper;
        }

        public void OnGet()
        {
            var accessToken = _apiWrapper.GetIdentity();

            var handler = new JwtSecurityTokenHandler();
            if (!(handler.ReadJwtToken(accessToken) is JwtSecurityToken jwt))
                throw new Exception("Unable to parse raw token.");
            Claims = jwt.Claims.ToList();
        }
    }
}