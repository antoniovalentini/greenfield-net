using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalentini.Expensi.IdentityServer.Data.Context;
using Avalentini.Expensi.Domain.Data.Repository;
using Avalentini.Expensi.IdentityServer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.IdentityServer.Data.Repository
{
    public class UsersRepository : IRepository<UserEntity>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UsersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<UserEntity>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<UserEntity> Get(string id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Id == int.Parse(id));
        }

        public void Add(UserEntity entity)
        {
            entity.CreationDate = DateTime.Now;
            _context.Users.Add(entity);
            _context.SaveChanges();
        }

        public void Edit(string id, UserEntity entity)
        {
            if (int.Parse(id) != entity.Id)
                return;

            var oldEntity = _context.Users.AsNoTracking().FirstOrDefault(e => e.Id == int.Parse(id));
            if (oldEntity == null)
                return;

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
                throw new Exception("Update concurrency exception. See inner ex for more details.", ex);
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

        public bool Exists(UserEntity user)
        {
            return _context.Users.Any(u => u.Firstname == user.Firstname && u.Lastname == user.Lastname);
        }

        public async Task<UserEntity> FindAsync(string usernameOrEmail)
        {
            var users = _context.Users.AsNoTracking().ToList();
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(e => e.Username == usernameOrEmail);
        }
    }
}
