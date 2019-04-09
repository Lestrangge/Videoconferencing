using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoconferencingBackend.DTO.Message.Response;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Services
{
    public class ChatService : IChatService
    {
        private readonly IUsersRepository _users;
        private readonly IGroupsRepository _groups;
        private readonly IMessagesRepository _messages;
        public ChatService(IUsersRepository users, IGroupsRepository groups, IMessagesRepository messages)
        {
            _users = users;
            _groups = groups;
            _messages = messages;
        }

        public async Task<Message> SendMessage(string text, string groupName, string senderLogin, IClientProxy clients)
        {
            var sender = await _users.Get(senderLogin);
            if (sender == null)
                throw new ArgumentException("Login not found");

            var group = await _groups.Get(groupName);
            if (group == null)
                throw new ArgumentException("Group not found");

            var participants = await _groups.GetGroupUsers(groupName);
            if (participants.Count(user => user.Login == senderLogin) < 1)
                throw new ArgumentException("User is not in a group");

            var message = new Message() { Group = group, Sender = sender, Text = text, Time = DateTime.Now};
            var incoming = new GroupMessageDto(message);
            await clients.SendAsync("IncomingMessage", incoming);
            return await _messages.Create(message);
        }
    }
}
