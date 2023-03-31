using ClaimsPay.Shared;
using System.Net;
using Microsoft.Extensions.Configuration;
using NLog;

namespace ClaimsPay.Filters
{
   
    public abstract class AuthenticationFilter : IEndpointFilter
    {
        protected readonly Microsoft.Extensions.Logging.ILogger Logger;
        private readonly string _methodName;
        private readonly string _filterType;
        public static IConfiguration configuration;
        private readonly IConfiguration _configuration;
        
        public AuthenticationFilter(IConfiguration configuration)
        {
             _configuration = configuration;
        }

        public AuthenticationFilter(ILoggerFactory loggerFactory, string filterType)
        {
            Logger = loggerFactory.CreateLogger<AuthenticationFilter>();
            _methodName = GetType().Name;
            _filterType = filterType;
        }
        public virtual async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {

            //var localize = _configuration.GetSection($"Logging:LogLevel");
            
            var headerToken = context.HttpContext.Request.Headers[_filterType];
            var authKeyPath = _filterType.ToString().Replace("_", ":");
            var matchingConfigToken = AppConfig.configuration?.GetSection($"Modules:{authKeyPath}")["AuthenticationFilter"];

            if (matchingConfigToken != null && headerToken != matchingConfigToken)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.HttpContext.Response.WriteAsync(AppConfig.configuration?.GetSection($"Modules:{authKeyPath}")["APIKeyErrorMessage"].ToString());
                await context.HttpContext.Response.CompleteAsync();
                return context;
            }

            return await next(context);
        }
    }
}
