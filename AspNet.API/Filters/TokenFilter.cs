using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AspNet.API.Filters
{
    public class TokenFilter : ActionFilterAttribute, IAsyncActionFilter
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                string authorizationKey = Environment.GetEnvironmentVariable("Key")!;
                string authorization = context.HttpContext.Request.Headers["Authorization"];

                if (authorization != authorizationKey)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                await next();
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}