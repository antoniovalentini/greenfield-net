using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Domain.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.Api.Data.Repository.Expenses
{
    public class ExpensesEfRepository : IRepository<Expense>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly int _userId;

        public ExpensesEfRepository(ApplicationDbContext context, IMapper mapper, int userId)
        {
            _context = context;
            _mapper = mapper;
            _userId = userId;
        }

        public async Task<IList<Expense>> GetAll()
        {
            var expenses = await _context.Expenses
                .Include(e => e.User)
                .AsNoTracking()
                .Where(e => e.User.Id == _userId)
                .ToListAsync();
            var result = new List<Expense>();
            foreach (var exp in expenses)
            {
                result.Add(_mapper.Map<Expense>(exp));
            }
            return result;
        }

        public async Task<Expense> Get(string id)
        {
            var entity = await _context.Expenses
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id && e.User.Id == _userId);
            return entity == null ? null : _mapper.Map<Expense>(entity);
        }

        public void Add(Expense expense)
        {
            var entity = _mapper.Map<ExpenseEntity>(expense);
            entity.User = _context.Users.SingleOrDefault(u => u.Id == _userId);
            entity.CreationDate = DateTime.Now;
            _context.Expenses.Add(entity);
            _context.SaveChanges();
        }

        public void Edit(string id, Expense expense)
        {
            if (id != expense.Id)
                return;

            var oldEntity = _context.Expenses
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id && e.User.Id == _userId);
            if (oldEntity == null)
                return;
            var entity = _mapper.Map<ExpenseEntity>(expense);

            // TODO: preserve cretion date (find a smarter way to do it)
            entity.CreationDate = oldEntity.CreationDate;
            //_context.Entry(entity).State = EntityState.Modified;
            _context.Expenses.Update(entity);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("Update concurrency exceptio. See inner ex for more details.", ex);
            }
        }

        public void Remove(string id)
        {
            var entity = _context.Expenses.Include(e => e.User).FirstOrDefault(e => e.Id == id && e.User.Id == _userId);
            // it's already checked inside the controller
            // two checkes means 2 calls... should we remove 1?
            if (entity == null)
                return;

            _context.Expenses.Remove(entity);
            _context.SaveChanges();
        }

        public bool Exists(Expense expense)
        {
            return _context.Expenses
                .Include(e => e.User)
                .Any(e => e.Id == expense.Id  && e.User.Id == _userId);
        }
    }
}
