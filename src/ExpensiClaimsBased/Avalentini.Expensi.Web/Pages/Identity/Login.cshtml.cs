using System;
using System.Threading.Tasks;
using Avalentini.Expensi.Web.Data;
using Avalentini.Expensi.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.Web.Pages.Identity
{
    public class LoginModel : PageModel
    {
        private readonly IApiWrapper _apiWrapper;

        [TempData]
        public string Message { get; set; }
        public bool HasMessage => !string.IsNullOrEmpty(Message);

        [BindProperty]
        public UserViewModel UserVm { get; set; }

        public LoginModel(IApiWrapper apiWrapper)
        {
            _apiWrapper = apiWrapper;
        }

        public IActionResult OnGet()
        {
            if (_apiWrapper.HasIdentity())
                return RedirectToPage("/Expenses/Index");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            // validate username and password
            var success = await _apiWrapper.LoginWithUsernameAndPassword(UserVm.Username, UserVm.Password);
            if (success) return RedirectToAction("Index", "Expenses");

            Message = "Wrong username or password";
            return Page();
        }
    }
}