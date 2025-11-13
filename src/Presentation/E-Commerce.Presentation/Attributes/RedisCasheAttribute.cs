using E_Commerce.ServicesAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace E_Commerce.Presentation.Attributes
{
    internal class RedisCasheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeInMin;
        public RedisCasheAttribute(int timeInMin = 10)
        {
            _timeInMin = timeInMin;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var casheService = context.HttpContext.RequestServices.GetRequiredService<ICasheService>();
            var casheKey = GetCacheKey(context.HttpContext.Request);
            var casheResult = await casheService.GetAsync(casheKey);

            if (!string.IsNullOrEmpty(casheResult))
            {
                context.Result = new ContentResult
                {
                    Content = casheResult,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await casheService.SetAsync(casheKey, executedContext.Result, TimeSpan.FromMinutes(_timeInMin));
            }
        }
        private string GetCacheKey(HttpRequest request)
        {
            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
