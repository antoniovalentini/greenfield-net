using System.Collections.Generic;
using System.Threading.Tasks;
using Avalentini.Expensi.Web.Model;

namespace Avalentini.Expensi.Web.Data
{
    public interface IExpensesRepository
    {
        Task<IEnumerable<ExpenseViewModel>> GetAll();
        Task<ExpenseViewModel> Get(string id);
        Task Add(ExpenseViewModel expense);
        Task Edit(string id, ExpenseViewModel expense);
        Task Remove(string id);
    }
}