using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Web.Data;
using Avalentini.Expensi.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.Web.Pages.Expenses
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public ExpenseViewModel Expense { get; set; }

        [TempData]
        public string Message { get; set; }

        private readonly IExpensesRepository _repo;

        public EditModel(IMapper mapper, IApiWrapper apiWrapper)
        {
            _repo = new ExpensesRepository(mapper, apiWrapper);
        }

        public async Task OnGet(string id)
        {
            Expense = await _repo.Get(id);
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            await _repo.Edit(Expense.Id, Expense);

            Message = "Expense updated successfully!";

            return RedirectToPage("Index");
        }
    }
}