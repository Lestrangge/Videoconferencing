using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VideoconferencingBackend.Interfaces.Repositories;

namespace VideoconferencingBackend.Middleware
{
    public class FcmTokenExtractorMiddleware
    {
        private readonly RequestDelegate _next;

        public FcmTokenExtractorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUsersRepository _users)
        {
            var token = context.Request.Headers["FcmToken"];
            if (!string.IsNullOrEmpty(token))
            {
                var guid = context.User.Identity.Name;
                var user = await _users.Get(guid);
                user.FcmToken = token;
                await _users.Update(user);
            }
            await _next(context);
        }

    }
}
