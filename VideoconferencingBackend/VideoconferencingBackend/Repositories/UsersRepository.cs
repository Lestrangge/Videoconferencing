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
                .Include(user => user.GroupInCall)
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

        public Task<User> GetByHandleId(long? handleId)
        {
            return _db.Users.Where(user => user.HandleId == handleId).FirstOrDefaultAsync();
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
            user.FcmToken = item.FcmToken ?? user.FcmToken;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateInCall(User item, Group groupInCall)
        {
            var user = _db.Users.Where(user1 => user1.UserGuid == item.UserGuid).Include(user1 => user1.GroupInCall).FirstOrDefault();
            if(groupInCall!=null)
                user.GroupInCall = _db.Groups.FirstOrDefault(group1 => group1.GroupGuid == groupInCall.GroupGuid) ;
            else
            {
                _db.Entry(user).Property("GroupInCallId").CurrentValue = null;
                _db.Entry(user).Property("GroupInCallId").IsModified = true;
            }
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
