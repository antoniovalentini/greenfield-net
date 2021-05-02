using System;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Web.Data;
using Avalentini.Expensi.Web.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.Web.Pages.Expenses
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public ExpenseViewModel Expense { get; set; }

        [TempData]
        public string Message { get; set; }

        private readonly IExpensesRepository _repo;

        public CreateModel(IMapper mapper, IApiWrapper apiWrapper)
        {
            _repo = new ExpensesRepository(mapper, apiWrapper);
        }

        public void OnGet()
        {
            Expense = new ExpenseViewModel {When = DateTime.Now};
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Expense.When = DateTime.Now;
            await _repo.Add(Expense);

            Message = "Expense created successfully!";

            return RedirectToPage("Index");
        }
    }
}