using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VideoconferencingBackend.Interfaces.Repositories;

namespace VideoconferencingBackend.Middleware
{
    public class FcmTokenExtractorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUsersRepository _users;

        public FcmTokenExtractorMiddleware(RequestDelegate next, IUsersRepository users)
        {
            _next = next;
            _users = users;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["FcmToken"];
            if (string.IsNullOrEmpty(token))
                return;
            var guid = context.User.Identity.Name;
            var user = await _users.Get(guid);
            user.FcmToken = token;
            await _users.Update(user);
        }

    }
}
