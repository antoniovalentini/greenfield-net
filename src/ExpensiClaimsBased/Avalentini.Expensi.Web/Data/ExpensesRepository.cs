using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Web.Model;
using Newtonsoft.Json;

namespace Avalentini.Expensi.Web.Data
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly IMapper _mapper;
        private readonly IApiWrapper _apiWrapper;

        public ExpensesRepository(IMapper mapper, IApiWrapper apiWrapper)
        {
            _mapper = mapper;
            _apiWrapper = apiWrapper;
        }

        public async Task<IEnumerable<ExpenseViewModel>> GetAll()
        {
            var response = await _apiWrapper.GetAll();
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to fetch expenses. Error: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var expenses = JsonConvert.DeserializeObject<IEnumerable<Expense>>(content);
            return expenses.Select(e => _mapper.Map<ExpenseViewModel>(e));
        }

        public async Task<ExpenseViewModel> Get(string id)
        {
            var response = await _apiWrapper.GetById(id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to fetch expense with id: {id}. Error: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var expense = JsonConvert.DeserializeObject<Expense>(content);
            return _mapper.Map<ExpenseViewModel>(expense);
        }

        public async Task Add(ExpenseViewModel expense)
        {
            var response = await _apiWrapper.PostExpense(new StringContent(JsonConvert.SerializeObject(expense), Encoding.Default, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to post expense with id: {expense.Id}. Error: {response.StatusCode}");
            }
        }

        public async Task Edit(string id, ExpenseViewModel expense)
        {
            var response = await _apiWrapper.PutExpense(id, new StringContent(JsonConvert.SerializeObject(expense), Encoding.Default, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to post expense with id: {expense.Id}. Error: {response.StatusCode}");
            }
        }

        public async Task Remove(string id)
        {
            var response = await _apiWrapper.DeleteExpense(id);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to delete expense with id: {id}. Error: {response.StatusCode}");
            }
        }
    }
}
