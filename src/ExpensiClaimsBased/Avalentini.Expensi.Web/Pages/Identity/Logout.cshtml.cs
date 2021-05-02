using Microsoft.AspNetCore.Mvc.RazorPages;
using Avalentini.Expensi.Web.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Avalentini.Expensi.Web.Pages.Identity
{
    public class LogoutModel : PageModel
    {
        private readonly IApiWrapper _apiWrapper;

        public LogoutModel(IApiWrapper apiWrapper)
        {
            _apiWrapper = apiWrapper;
        }

        public async Task<IActionResult> OnGet()
        {
            await _apiWrapper.Logout();
            return RedirectToPage("/Index");
        }
    }
}