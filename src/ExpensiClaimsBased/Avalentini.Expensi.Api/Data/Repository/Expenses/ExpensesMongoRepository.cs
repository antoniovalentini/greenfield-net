using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Domain.Data.Repository;
using MongoDB.Driver;

namespace Avalentini.Expensi.Api.Data.Repository.Expenses
{
    public class ExpensesMongoRepository : IRepository<Expense>
    {
        private readonly IMongoCollection<ExpensesPerUser> _collection;
        private readonly ExpensesPerUser _expensesPerUser;
        private readonly IMapper _mapper;

        public ExpensesMongoRepository(IMongoCollection<ExpensesPerUser> collection, IMapper mapper, int userId)
        {
            _mapper = mapper;
            _collection = collection;
            var element = collection.Find(e => e.UserId == userId);
            _expensesPerUser = element.CountDocuments() > 0 ? element.ToList().First() : new ExpensesPerUser();
        }

        public async Task<IList<Expense>> GetAll()
        {
            var result = new List<Expense>();
            foreach (var exp in _expensesPerUser.Expenses)
            {
                result.Add(_mapper.Map<Expense>(exp));
            }
            return await Task.FromResult(result);
        }

        public async Task<Expense> Get(string id)
        {
            var entity = _expensesPerUser.Expenses.FirstOrDefault(e => e.ExpenseId == id.ToString());
            return await Task.FromResult(entity == null ? null : _mapper.Map<Expense>(entity));
        }

        public void Add(Expense expense)
        {
            var entity = _mapper.Map<ExpenseMongoEntity>(expense);
            entity.CreationDate = DateTime.Now;
            entity.ExpenseId = Guid.NewGuid().ToString();
            _expensesPerUser.Expenses.Add(entity);
            _collection.ReplaceOne(eu => eu.UserId == _expensesPerUser.UserId, _expensesPerUser);
        }

        public void Edit(string id, Expense expense)
        {
            if (id != expense.Id)
                return;

            var oldEntity = _expensesPerUser.Expenses.FirstOrDefault(e => e.ExpenseId == id);
            if (oldEntity == null)
                return;
            var entity = _mapper.Map<ExpenseMongoEntity>(expense);

            // TODO: preserve cretion date (find a smarter way to do it)
            entity.CreationDate = oldEntity.CreationDate;

            _expensesPerUser.Expenses.Remove(oldEntity);
            _expensesPerUser.Expenses.Add(entity);
            _collection.ReplaceOne(eu => eu.UserId == _expensesPerUser.UserId, _expensesPerUser);
        }

        public void Remove(string id)
        {
            var entity = _expensesPerUser.Expenses.FirstOrDefault(e => e.ExpenseId == id);
            // it's already checked inside the controller
            // two checkes means 2 calls... should we remove 1?
            if (entity == null)
                return;

            _expensesPerUser.Expenses.Remove(entity);
            _collection.ReplaceOne(eu => eu.UserId == _expensesPerUser.UserId, _expensesPerUser);
        }

        public bool Exists(Expense expense)
        {
            return _expensesPerUser.Expenses.Any(e => e.ExpenseId == expense.Id.ToString());
        }
    }
}
