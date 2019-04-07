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
        private readonly int _pageSize;

        public GroupsController(IGroupsRepository groupsRepository, IConfiguration config)
        {
            _groupsRepository = groupsRepository;
            _pageSize = int.Parse(config["GroupsFindPageSize"]);
        }

        [HttpGet]
        [Route("find")]
        public async Task<IActionResult> FindGroups(string name, int? page, int? pageSize)
        {
            return new OkObjectResult((await _groupsRepository.Find(name, page ?? 0, pageSize ?? _pageSize))
                .Select(group => new GroupFoundDto(group)));
        }

        [HttpGet]
        [Authorize]
        [Route("created")]
        public async Task<IActionResult> GetCreatedGroups(int? page, int? pageSize)
        {
            var me = HttpContext.User.Identity.Name;
            return new OkObjectResult((await _groupsRepository.GetCreatedGroups(me, page??0, pageSize ?? _pageSize))
                .Select(group=> new GroupFoundDto(group)));
        }

        [HttpPost]
        [Authorize]
        [Route("join")]
        public async Task<IActionResult> JoinGroup(string groupName)
        {
            var me = HttpContext.User.Identity.Name;
            try
            {
                return new OkObjectResult(new GroupFoundDto(await _groupsRepository.AddToGroup(me, groupName)));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);   
            }
        }

        [HttpGet]
        [Authorize]
        [Route("joined")]
        public async Task<IActionResult> GetJoinedGroups(int? page, int? pageSize)
        {
            var me = HttpContext.User.Identity.Name;
            return new OkObjectResult((await _groupsRepository.GetUsersGroups(me, page ?? 0, pageSize ?? _pageSize))
                .Select(group => new GroupFoundDto(group)));
        }

        [HttpGet]
        [Authorize]
        [Route("participants")]
        public async Task<IActionResult> GetGroupParticipants(string groupName, int? page, int? pageSize)
        {
            return new OkObjectResult((await _groupsRepository.GetGroupUsers(groupName, page ?? 0, pageSize ?? _pageSize))
                .Select(user => new UserFoundDto(user)));
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public async Task<IActionResult> Post([FromBody]GroupCreateDto @group)
        {
            if(!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState.Values.Select(value => value.Errors.FirstOrDefault()).FirstOrDefault()?.ErrorMessage);
            if (await _groupsRepository.Get(@group.Name) != null) 
                return new BadRequestObjectResult("Group name is already taken");
            try
            {
                var gr = await _groupsRepository.CreateWithOwner(@group, HttpContext.User.Identity.Name);
                return new OkObjectResult(new GroupFoundDto(gr));
            }
            catch (ArgumentException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}