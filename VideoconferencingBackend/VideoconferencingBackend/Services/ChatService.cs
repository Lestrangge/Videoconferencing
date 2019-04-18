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
        private readonly IPushMessagesService _pusher;
        public ChatService(IUsersRepository users, IGroupsRepository groups, IMessagesRepository messages, IPushMessagesService pusher)
        {
            _users = users;
            _groups = groups;
            _messages = messages;
            _pusher = pusher;
        }

        public async Task<Message> SendMessage(string text, string groupGuid, string userGuid, IClientProxy clients)
        {
            var sender = await _users.Get(userGuid);    
            if (sender == null)
                throw new ArgumentException("Login not found");

            var group = await _groups.Get(groupGuid);
            if (group == null)
                throw new ArgumentException("Group not found");

            var participants = await _groups.GetGroupUsers(groupGuid);
            if (participants.Count(user => user.UserGuid == sender.UserGuid) < 1)
                throw new ArgumentException("User is not in a group");

            var message = new Message() { Group = group, Sender = sender, Text = text, Time = DateTime.Now};
            var m = await _messages.Create(message);

            var incoming = new GroupMessageDto(message);
            var users = await _groups.GetGroupUsers(groupGuid);
            foreach (var user in users)
                await _pusher.ChatMessage(incoming, user, group, clients);
            return message;
        }
    }
}
