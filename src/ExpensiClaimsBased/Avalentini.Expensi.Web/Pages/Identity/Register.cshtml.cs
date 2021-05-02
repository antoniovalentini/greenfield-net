using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Web.Data;
using Avalentini.Expensi.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.Web.Pages.Identity
{
    public class RegisterModel : PageModel
    {
        private readonly IApiWrapper _apiWrapper;
        private readonly IMapper _mapper;

        [BindProperty]
        public UserViewModel UserVm { get; set; }

        public RegisterModel(IApiWrapper apiWrapper, IMapper mapper)
        {
            _apiWrapper = apiWrapper;
            _mapper = mapper;
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
            var apiUser = _mapper.Map<Api.Contracts.Models.User>(UserVm);
            await _apiWrapper.RegisterUser(apiUser);

            return RedirectToAction("Index", "Expenses");
        }
    }
}