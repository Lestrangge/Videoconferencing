using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.DTO.Group.Requests;
using VideoconferencingBackend.DTO.Group.Responses;
using VideoconferencingBackend.DTO.User.Responses;
using VideoconferencingBackend.Interfaces.Repositories;

namespace VideoconferencingBackend.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class GroupsController : Controller
    {
        private readonly IGroupsRepository _groupsRepository;
        private readonly IMessagesRepository _messagesRepository;
        private readonly int _pageSize;

        public GroupsController(IGroupsRepository groupsRepository, IConfiguration config, IMessagesRepository messagesRepository)
        {
            _groupsRepository = groupsRepository;
            _messagesRepository = messagesRepository;
            _pageSize = int.Parse(config["GroupsFindPageSize"]);
        }


        /// <summary>
        /// Returns enumerable of groups containing pattern. 
        /// </summary>
        /// <param name="pattern">pattern contained in group name</param>
        /// <param name="page">optional: page number (base 0)</param>
        /// <param name="pageSize">optional: page size (base 10)</param>
        /// <returns>Enumerable of matching groups</returns>
        /// <response code="200">Array of matching groups</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [Route("find")]
        public async Task<IActionResult> FindGroups(string pattern, int? page, int? pageSize)
        {
            return new OkObjectResult(new {Groups = (await _groupsRepository.Find(pattern, page, pageSize))
                .Select(group => new GroupFoundDto(group))});
        }

        /// <summary>
        /// Enumerable of groups, created by user
        /// </summary>
        /// <param name="page">optional: page number (base 0)</param>
        /// <param name="pageSize">optional: page size (base 10)</param>
        /// <returns>Enumerable of groups, created by user</returns>
        /// <response code="200">Enumerable of matching groups</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [Route("created")]
        public async Task<IActionResult> GetCreatedGroups(int? page, int? pageSize)
        {
            var me = HttpContext.User.Identity.Name;
            return new OkObjectResult(new {Groups = (await _groupsRepository.GetCreatedGroups(me, page, pageSize))
                .Select(async group =>
                {
                    var lastMessage = (await _messagesRepository.GetMessagesFromGroup(group.GroupGuid, 0, 1)).FirstOrDefault();
                    return new GroupFoundWithMessageDto(group, lastMessage);
                })
            });
        }

        /// <summary>
        /// Join the group, specified by group name
        /// </summary>
        /// <param name="groupGuid">Name of the group to join</param>
        /// <returns>Joined group</returns>
        /// <response code="200">Joined group</response>
        /// <response code="400">Group not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize]
        [Route("join")]
        public async Task<IActionResult> JoinGroup(string groupGuid)
        {
            var me = HttpContext.User.Identity.Name;
            try
            {
                return new OkObjectResult(new GroupFoundDto(await _groupsRepository.AddToGroup(me, groupGuid)));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);   
            }
        }

        /// <summary>
        /// Join the group, specified by group name
        /// </summary>
        /// <param name="joinRequest"></param>
        /// <returns>Joined group</returns>
        /// <response code="200">Joined group</response>
        /// <response code="400">Group not found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize]
        [Route("invite")]
        public async Task<IActionResult> InviteGroup([FromBody] GroupJoinDto joinRequest)
        {
            try
            {
                return new OkObjectResult(new GroupFoundDto(await _groupsRepository.AddToGroup(joinRequest.UserGuid, joinRequest.GroupGuid)));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Get users groups
        /// </summary>
        /// <param name="page">optional: page number (base 0)</param>
        /// <param name="pageSize">optional: page size (base 10)</param>
        /// <returns>Enumerable of joined groups</returns>
        /// <response code="200">Enumerable of joined groups</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [Route("joined")]
        public async Task<IActionResult> GetJoinedGroups(int? page, int? pageSize)
        {
            var me = HttpContext.User.Identity.Name;
            return new OkObjectResult(new {Groups = (await _groupsRepository.GetUsersGroups(me, page, pageSize))
                .Select(async group =>
                {
                    var lastMessage = (await _messagesRepository.GetMessagesFromGroup(group.GroupGuid, 0, 1)).FirstOrDefault();
                    return new GroupFoundWithMessageDto(group, lastMessage);
                })
            });
        }

        /// <summary>
        /// Get group participants
        /// </summary>
        /// <param name="pageSize">optional: page size (base 10)</param>
        /// <param name="groupGuid"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        /// <response code="200">Enumerable of participants of the group</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Authorize]
        [Route("participants")]
        public async Task<IActionResult> GetGroupParticipants(string groupGuid, int? page, int? pageSize)
        {   
            return new OkObjectResult(new {Users = (await _groupsRepository.GetGroupUsers(groupGuid, page, pageSize))
                .Select(user => new UserFoundDto(user))});
        }

        /// <summary>
        /// Create a group
        /// </summary>
        /// <param name="group">group to create</param>
        /// <returns>Created group</returns>
        /// <response code="200">Created group</response>
        /// <response code="400">Group name is taken</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Post([FromBody]GroupCreateDto @group)
        {
            if(!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.Select(value => value.Errors.FirstOrDefault()).FirstOrDefault()?.ErrorMessage);
            if (await _groupsRepository.GetByName(@group.Name) != null) 
                return new BadRequestObjectResult("Group name is already taken");
            try
            {
                var gr = await _groupsRepository.CreateWithOwner(@group, HttpContext.User.Identity.Name);
                if (group.Users != null) 
                    foreach(var user in group.Users)
                    {
                        await _groupsRepository.AddToGroup(user, gr.GroupGuid);
                    }
                return new OkObjectResult(new GroupFoundDto(gr));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("leave")]
        public async Task<IActionResult> LeaveGroup(string groupGuid)
        {
            try
            {
                var deleted = await _groupsRepository.RemoveFromGroup(HttpContext.User.Identity.Name, groupGuid);
                return new OkObjectResult(new GroupFoundDto(deleted));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}