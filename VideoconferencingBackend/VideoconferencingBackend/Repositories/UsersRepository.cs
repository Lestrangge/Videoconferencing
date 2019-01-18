using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VideoconferencingBackend.Interfaces;
using VideoconferencingBackend.Models;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private readonly DatabaseContext _db;

        public UsersRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<User>> All()
        {
            return await _db.Users.Include(x => x.Role).ToListAsync();
        }

        public async Task<User> Create(User item)
        {
            await _db.Users.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public Task<User> Delete(User item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Task<User> Get(int id)
        {
            return _db.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<User> Get(string login)
        {
            return _db.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<User> Update(User item)
        {
            var user = await Get(item.Login);
            if (user == null)
                return user;
            user.Name = item.Name ?? user.Name;
            user.HandleId = item.HandleId ?? user.HandleId;
            user.SessionId = item.SessionId ?? user.SessionId;
            user.ImageLink = item.ImageLink ?? user.ImageLink;
            user.Password = item.Password ?? user.Password;
            user.Surname = item.Surname ?? user.Surname;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
