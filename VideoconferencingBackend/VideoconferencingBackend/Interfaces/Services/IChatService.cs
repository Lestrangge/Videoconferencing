﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Services
{
    public interface IChatService
    {
        Task<Message> SendMessage(string message, string groupName, string senderLogin, IClientProxy group);
    }
}
