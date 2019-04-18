using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VideoconferencingBackend.DTO.Message.Response;
using VideoconferencingBackend.Models.DBModels;

namespace VideoconferencingBackend.Interfaces.Services
{
    public interface IPushMessagesService
    {
        Task ChatMessage(GroupMessageDto message, User user, Group group, IClientProxy clients);
        Task CallStarted(User user, Group group, IClientProxy clients); 
    }
}
