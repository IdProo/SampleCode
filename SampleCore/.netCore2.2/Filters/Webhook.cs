using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SampleCore.Filters
{
    public class WebhookFilters : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string key = context.HttpContext.Request.Headers["W-Key"].ToString();
            bool isValid = key.Equals(Startup.webKey);
            if (!isValid)
            {
                context.Result = new BadRequestObjectResult("Bad request!");
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
