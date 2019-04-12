﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Models;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Utils;

namespace VideoconferencingBackend.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DatabaseContext _db;

        public UsersRepository(DatabaseContext db)
        {
            _db = db;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<User>> All()
        {
            return await _db.Users.ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<User> Create(User item)
        {
            item.UserGuid = Guid.NewGuid().ToString();
            await _db.Users.AddAsync(item);
            await _db.SaveChangesAsync();
            return item;
        }

        ///<inheritdoc/>
        public Task<User> Delete(User item)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        ///<inheritdoc/>
        public Task<User> Get(int id)
        {
            return _db.Users.Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public Task<User> Get(string userGuid)
        {
            return _db.Users.Where(x => x.UserGuid == userGuid)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetByLogin(string login)
        {
            return _db.Users.Where(x => x.Login == login)
                .FirstOrDefaultAsync();
        }

        public Task<User> GetBySessionId(long? responseSessionId)
        {
            return _db.Users.Where(user => user.SessionId == responseSessionId).FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task<User> Update(User item)
        {
            var user = await Get(item.UserGuid);
            if (user == null)
                throw new KeyNotFoundException("No user found with such name");
            user.Name = item.Name ?? user.Name;
            user.HandleId = item.HandleId ?? user.HandleId;
            user.SessionId = item.SessionId ?? user.SessionId;
            user.Password = item.Password ?? user.Password;
            user.Surname = item.Surname ?? user.Surname;
            user.AvatarLink = item.AvatarLink ?? user.AvatarLink;
            user.ConnectionId = item.ConnectionId ?? user.ConnectionId;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<User>> Find(string name, int? page = null, int? pageSize = null)
        {
            return await _db.Users
                .Where(el => el.Login.Contains(name))
                .Paginate(page, pageSize)
                .ToListAsync();
        }

    }
}
