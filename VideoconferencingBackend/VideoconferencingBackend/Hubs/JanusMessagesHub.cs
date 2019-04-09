﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NLog;
using VideoconferencingBackend.DTO.Hub.ServerResponse;
using VideoconferencingBackend.DTO.Message.Response;
using VideoconferencingBackend.Interfaces.Repositories;
using VideoconferencingBackend.Interfaces.Services;
using VideoconferencingBackend.Interfaces.Services.Janus;

namespace VideoconferencingBackend.Hubs
{
    public class JanusMessagesHub : Hub
    {
        private readonly IJanusApiService _janus;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IGroupsRepository _groups;
        private readonly IChatService _chats;

        public JanusMessagesHub(IJanusApiService janus, IGroupsRepository groups, IChatService chats)
        {
            _janus = janus;
            _groups = groups;
            _chats = chats;
        }

        public string TestFree()
        {
            return $"Ur cool, {Context.User.Identity.Name}, u made it! Now invoke Test to check whether u can access auth-only requests or not";
        }

        public override async Task OnConnectedAsync()
        {
            var me = Context.User.Identity.Name;
            var groups = await _groups.GetUsersGroups(me, 0, await _groups.GetUsersGroupsLength(me));
            foreach (var @group in groups)
                await Groups.AddToGroupAsync(Context.ConnectionId, @group.GroupGuid);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var me = Context.User.Identity.Name;
            var groups = await _groups.GetUsersGroups(me, 0, await _groups.GetUsersGroupsLength(me));
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
        public async Task<HubResponse> InitiateCall(string groupGuid)
        {
            var jsep = await  _janus.InitiateCall(groupGuid);
            await Clients.Group(groupGuid).SendAsync("CallStarted", groupGuid);
            return new HubSuccessResponse(jsep);
        }

        [Authorize]
        public async Task<HubResponse> SendChatMessage(string groupGuid, string text)
        {   
            var userGuid = Context.User.Identity.Name;
            try
            {
                var message = await _chats.SendMessage(text, groupGuid, userGuid, Clients.Group(groupGuid));
                return new HubSuccessResponse(new GroupMessageDto(message));
            }
            catch (ArgumentException ex)
            {
                return new HubErrorResponse(1, ex.Message);
            }
        }
    }
}
