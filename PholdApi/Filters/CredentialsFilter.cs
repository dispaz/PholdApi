using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using PholdApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PholdApi.Filters
{
    public class CredentialsFilter : IAsyncActionFilter
    {

        private readonly ICredentialsService _credentialsService;
        private readonly ILogger _logger;

        public CredentialsFilter(ICredentialsService credentialsService, ILogger<CredentialsFilter> logger)
        {
            _credentialsService = credentialsService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            if(request.Query.ContainsKey("api-key"))
            {
                var apiKey = request.Query["api-key"];
                var isKeyValid = await _credentialsService.CheckApiKey(apiKey);
                if (!isKeyValid)
                {
                    _logger.LogInformation($"action=filter api-key={apiKey} msg=Invalid api key");
                    context.Result = new UnauthorizedObjectResult("Invalid api key");
                    return;
                }
            }
            else
            {
                _logger.LogInformation($"action=filter msg=Api key is empty");
                context.Result = new UnauthorizedObjectResult("Enter api key");
                return;
            }
            await next();
        }
    }
}
