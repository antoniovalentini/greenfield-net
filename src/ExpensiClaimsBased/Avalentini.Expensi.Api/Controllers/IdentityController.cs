using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Avalentini.Expensi.Api.Controllers
{
    /// <summary>
    /// This controller will be used later to test the authorization requirement,
    /// as well as visualize the claims identity through the eyes of the API.
    /// </summary>
    [Route("identity")]
    [ApiController]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}