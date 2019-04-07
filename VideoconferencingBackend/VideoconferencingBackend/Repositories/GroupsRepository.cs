﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Models;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Repositories
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly DatabaseContext _db;
        public GroupsRepository(DatabaseContext db)
        {
            _db = db;
        }
        public void Dispose()
        {
            _db.Dispose();
        }
        ///<inheritdoc/>
        public async Task<IEnumerable<Group>> All()
        {
            return await _db.Groups.ToListAsync();
        }
        ///<inheritdoc/>
        public Task<Group> Get(int id)
        {
            return _db.Groups.Where(group => group.Id == id).FirstOrDefaultAsync();
        }
        ///<inheritdoc/>
        public Task<Group> Get(string name)
        {
            return _db.Groups.Where(group => group.Name == name).FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task<Group> Create(Group item)
        {
            await _db.Groups.AddAsync(item);
            await _db.GroupUsers.AddAsync(new GroupUser { Group = item, User = item.Creator});
            await _db.SaveChangesAsync();
            return item;
        }

        ///<inheritdoc/>
        public Task<Group> Delete(Group item)
        {
            throw new NotImplementedException();
        }

        ///<inheritdoc/>
        public async Task<Group> Update(Group item)
        {
            var @group = await Get(item.Name);
            if(@group == null)
                throw new KeyNotFoundException("No group found with such name");
            @group.Name = item.Name ?? @group.Name;
            @group.Description = item.Description ?? @group.Description;
            @group.InCall = item.InCall ?? group.InCall;
            _db.Groups.Update(@group);
            await _db.SaveChangesAsync();
            return @group;
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<Group>> Find(string name, int page, int pageSize)
        {
            return await _db.Groups
                .Where(el => el.Name.Contains(name))
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Group>> GetCreatedGroups(string name, int page, int pageSize)
        {
            return await _db.Groups
                .Include(group => group.Creator)
                .Where(group => group.Creator.Login == name)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Group> AddToGroup(string userLogin, string groupName)
        {
            var user = await _db.Users.Where(item => item.Login == userLogin).FirstOrDefaultAsync();
            var group = await _db.Groups.Where(item => item.Name == groupName).FirstOrDefaultAsync();
            if(user == null)
                throw new ArgumentException("User not found");
            if(group == null)
                throw new ArgumentException("Group not found");
            await _db.GroupUsers.AddAsync(new GroupUser {Group = group, User = user});
            await _db.SaveChangesAsync();
            return group;
        }
        
        public async Task<Group> CreateWithOwner(Group item, string ownerLogin)
        {
            item.Creator = await _db.Users.Where(user => user.Login == ownerLogin).FirstOrDefaultAsync() 
                           ?? throw new ArgumentException("Current user not found");
            return await Create(item);
        }

        public async Task<IEnumerable<Group>> GetUsersGroups(string userLogin, int page, int pageSize)
        {
            var user = await _db.Users.Where(item => item.Login == userLogin).FirstOrDefaultAsync();
            return await _db.GroupUsers.Where(groupUser => groupUser.User == user)
                .Include(groupUser => groupUser.Group)
                .Select(groupUser => groupUser.Group)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetGroupUsers(string groupName, int page, int pageSize)
        {
            var group = await _db.Groups.Where(group1 => group1.Name == groupName).FirstOrDefaultAsync();
            return await _db.GroupUsers.Where(groupUser => groupUser.Group == group)
                .Include(groupUser => groupUser.User)
                .Select(groupUser => groupUser.User)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
