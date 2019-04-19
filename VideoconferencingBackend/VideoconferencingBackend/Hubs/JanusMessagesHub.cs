using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using NLog;
using VideoconferencingBackend.DTO.Hub.ServerEvents;
using VideoconferencingBackend.DTO.Hub.ServerRequest;
using VideoconferencingBackend.DTO.Hub.ServerResponse;
using VideoconferencingBackend.DTO.Message.Response;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services;
using VideoconferencingBackend.Interfaces.Services.Janus;
using VideoconferencingBackend.Models.Janus.PluginApi.PluginRequest;

namespace VideoconferencingBackend.Hubs
{
    [Authorize]
    public class JanusMessagesHub : Hub
    {
        private readonly IJanusApiService _janus;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGroupsRepository _groups;
        private readonly IChatService _chats;
        private readonly IUsersRepository _users;
        private readonly IPushMessagesService _pusher;

        public JanusMessagesHub(IJanusApiService janus, IGroupsRepository groups, IChatService chats, IUsersRepository users, IPushMessagesService pusher)
        {
            _janus = janus;
            _groups = groups;
            _chats = chats;
            _users = users;
            _pusher = pusher;
        }

        public string TestFree()
        {
            return $"Ur cool, {Context.User.Identity.Name}, u made it! Now invoke Test to check whether u can access auth-only requests or not";
        }

        public override async Task OnConnectedAsync()
        {
            var me = Context.User.Identity.Name;
            _logger.Trace($"User {me} connected. ");
            var groups = await _groups.GetUsersGroups(me, 0, await _groups.GetUsersGroupsLength(me));
            var user = await _users.Get(me);
            _logger.Trace($"User {user.Login} found. Connection id: {Context.ConnectionId}");
            user.ConnectionId = Context.ConnectionId;
            await _users.Update(user);
            foreach (var @group in groups)
                await Groups.AddToGroupAsync(Context.ConnectionId, @group.GroupGuid);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var me = Context.User.Identity.Name;
            _logger.Trace($"User {me} disconnected");
            var groups = await _groups.GetUsersGroups(me, 0, await _groups.GetUsersGroupsLength(me));
            var user = await _users.Get(me);
            user.ConnectionId = "";
            try
            {
                await _janus.Destroy();
                user.ConnectionId = null;
                await _users.Update(user);
                await _janus.UpdateInCall(me);
            }
            catch (Exception ex)
            {

            }
            foreach (var @group in groups)
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, @group.GroupGuid);
            await base.OnDisconnectedAsync(exception);
        }

        [Authorize]
        public string Test()
        {
            return "Ur cool, u got it, " + Context.User.Identity.Name;
        }

        [Authorize]
        public async Task<HubResponse> InitiateCall(InitiateCallRequestDto initiateCallRequest)
        {
            _logger.Trace($"InitiateCall reqest: {JsonConvert.SerializeObject(initiateCallRequest)}");
            try
            {
                var answer = await _janus.InitiateCall(initiateCallRequest.GroupGuid, initiateCallRequest.Offer);
                var user = await _users.Get(Context.User.Identity.Name);
                var group = await _groups.Get(initiateCallRequest.GroupGuid);
                await _pusher.CallStarted(user,group, Clients.All);
                await _users.UpdateInCall(user, group);
                await _groups.UpdateInCall(group, true);
                return new HubSuccessResponse(answer);
            }
            catch (Exception ex)
            {
                return new HubErrorResponse(1, ex.Message);
            }

        }
        [Authorize]
        public async Task<HubResponse> Trickle(TrickleCandidateReceivedDto iceCandidate)
        {
            _logger.Trace($"Trickle request {iceCandidate}");
            try
            {
                
                var res = await _janus.Trickle(iceCandidate);
                return new HubSuccessResponse(res);

            }
            catch (Exception ex)
            {
                return new HubErrorResponse(1, ex.Message);
            }
        }

        [Authorize]
        public async Task<HubResponse> AnswerNewPublisher(AnswerNewPublisherRequsetDto answerNewPublisherRequest)
        {
            try
            {
                var res = await _janus.StartPeerConnection(answerNewPublisherRequest.Answer,
                    answerNewPublisherRequest.HandleId);
                return new HubSuccessResponse(res);
            }
            catch (Exception ex)
            {
                return new HubErrorResponse(1, ex.Message);
            }
        }

        [Authorize]
        public async Task<HubResponse> SendChatMessage(string groupGuid, string text)
        {   
            var userGuid = Context.User.Identity.Name;
            try
            {

                var message = await _chats.SendMessage(text, groupGuid, userGuid, Clients.All);
                return new HubSuccessResponse(new GroupMessageDto(message));
            }
            catch (ArgumentException ex)
            {
                return new HubErrorResponse(1, ex.Message);
            }
        }
    }
}
