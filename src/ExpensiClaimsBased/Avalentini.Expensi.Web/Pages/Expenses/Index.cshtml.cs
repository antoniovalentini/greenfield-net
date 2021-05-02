using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Web.Data;
using Avalentini.Expensi.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.Web.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        public IList<ExpenseViewModel> Expenses { get; set; }
        public bool HasExpenses => Expenses != null && Expenses.Count > 0;

        [TempData]
        public string Message { get; set; }

        public bool HasMessage => !string.IsNullOrEmpty(Message);

        private readonly IExpensesRepository _repo;

        public IndexModel(IMapper mapper, IApiWrapper apiWrapper)
        {
            _repo = new ExpensesRepository(mapper, apiWrapper);
        }

        public async Task OnGet()
        {
            Expenses = (await _repo.GetAll()).ToList();
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            await _repo.Remove(id);
            Message = "Expense deleted successfully!";
            return RedirectToPage();
        }
    }
}