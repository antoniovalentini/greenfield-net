using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Api.Data.Entities;
using Avalentini.Expensi.Domain.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.Api.Data.Repository.Users
{
    public class UsersRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IList<User>> GetAll()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            var result = new List<User>();
            foreach (var u in users)
            {
                result.Add(_mapper.Map<User>(u));
            }
            return result;
        }

        public async Task<User> Get(string id)
        {
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == int.Parse(id));
            return entity == null ? null : _mapper.Map<User>(entity);
        }

        public void Add(User expense)
        {
            var entity = _mapper.Map<UserEntity>(expense);
            entity.CreationDate = DateTime.Now;
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Edit(string id, User user)
        {
            if (int.Parse(id) != user.Id)
                return;

            var oldEntity = _context.Users.AsNoTracking().FirstOrDefault(e => e.Id == int.Parse(id));
            if (oldEntity == null)
                return;
            var entity = _mapper.Map<UserEntity>(user);

            // TODO: preserve cretion date (find a smarter way to do it)
            entity.CreationDate = oldEntity.CreationDate;
            //_context.Entry(entity).State = EntityState.Modified;
            _context.Users.Update(entity);

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
            var entity = _context.Users.Find(id);
            // it's already checked inside the controller
            // two checkes means 2 calls... should we remove 1?
            if (entity == null)
                return;

            _context.Users.Remove(entity);
            _context.SaveChanges();
        }

        public bool Exists(User user)
        {
            return _context.Users.Any(u => u.Firstname == user.Firstname && u.Lastname == user.Lastname);
        }
    }
}
