using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using VideoconferencingBackend.DTO.Message.Response;
using VideoconferencingBackend.Interfaces.Repositories;

namespace VideoconferencingBackend.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class MessagesController : Controller
    {
        private readonly IMessagesRepository _messages;
        private readonly int _messagesOnPage;
        public MessagesController(IMessagesRepository messages, IConfiguration config)
        {
            _messages = messages;
            _messagesOnPage = int.Parse(config["MessagesOnPage"]);
        }

        [Authorize]
        [Route("get_by_group")]
        public async Task<IActionResult> GetGroupMessages(string groupName, int? page, int? pageNumber)
        {
             return new OkObjectResult(new {Messages = (await _messages.GetMessagesFromGroup(groupName, page ?? 0, pageNumber ?? _messagesOnPage))
                 .Select(message => new GroupMessageDto(message))});
        }
    }
}