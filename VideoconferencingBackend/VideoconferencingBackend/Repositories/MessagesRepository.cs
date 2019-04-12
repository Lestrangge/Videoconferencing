using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Models;
using VideoconferencingBackend.Models.DBModels;
using VideoconferencingBackend.Utils;

namespace VideoconferencingBackend.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly DatabaseContext _db;

        public MessagesRepository(DatabaseContext db)
        {
            _db = db;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task<IEnumerable<Message>> GetMessagesFromGroup(string groupGuid, int? page = null, int? pageSize = null)
        {
            return await _db.Messages
                .Include(message => message.Group)
                .Where(message => message.Group.GroupGuid == groupGuid)
                .Include(message => message.Sender)
                .OrderByDescending(message => message.Time)
                .Paginate(page, pageSize)
                .ToListAsync();
        }

        public Task<IEnumerable<Message>> All()
        {
            throw new System.NotImplementedException();
        }

        public Task<Message> Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Message> Get(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Message> Create(Message item)
        {
            try
            {
                _db.Attach(item.Group);
                _db.Attach(item.Sender);
            }
            catch (InvalidOperationException ex)
            {

            }
            var created = await _db.Messages.AddAsync(item);
            await _db.SaveChangesAsync();
            return created.Entity;
        }

        public Task<Message> Delete(Message item)
        {
            throw new System.NotImplementedException();
        }

        public Task<Message> Update(Message item)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Message>> Find(string name, int? page = null, int? pageSize = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
