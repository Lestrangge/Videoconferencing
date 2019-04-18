using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoconferencingBackend.DTO.Hub.ServerEvents;
using VideoconferencingBackend.DTO.Message.Response;
using VideoconferencingBackend.Interfaces.Services;
using VideoconferencingBackend.Models.DBModels;
using Message = FirebaseAdmin.Messaging.Message;

namespace VideoconferencingBackend.Services
{
    public class FcmPushMessagesService : IPushMessagesService
    {
        public async Task ChatMessage(GroupMessageDto payload, User user, Group group, IClientProxy clients)
        {
            if (!string.IsNullOrEmpty(user.ConnectionId))
                await clients.SendAsync("IncomingMessage", payload);

            if (string.IsNullOrEmpty(user.FcmToken))
                return;
            var serializedMessage = JsonConvert.SerializeObject(payload); 

            var message = new Message()
            {
                Token = user.FcmToken,
                Data = new Dictionary<string, string>()
                {
                    { "message", serializedMessage }
                },
                Notification = new Notification()
                {
                    Title = group.Name,
                    Body = payload.Text
                }
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        public async Task CallStarted(User user, Group @group, IClientProxy clients)    
        {
            var payload = new CallStartedDto(user, @group); 
            if (!string.IsNullOrEmpty(user.ConnectionId))
                await clients.SendAsync("CallStarted", payload);
            if (string.IsNullOrEmpty(user.FcmToken))
                return;
            var serializedMessage = JsonConvert.SerializeObject(payload);
            var message = new Message()
            {
                Token = user.FcmToken,
                Data = new Dictionary<string, string>()
                {
                    { "message", serializedMessage}
                },
                Notification = new Notification()
                {
                    Title = group.Name,
                    Body = user.Login
                }
            };
            var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
