using ClaimsPay.Shared;
using System.Net;

namespace ClaimsPay.Filters
{

    public abstract class IPFilter : IEndpointFilter
    {
        protected readonly ILogger Logger;
        private readonly string _methodName;
        private readonly string _filterType;

        public IPFilter(ILoggerFactory loggerFactory, string filterType)
        {
            Logger = loggerFactory.CreateLogger<IPFilter>();
            _methodName = GetType().Name;
            _filterType = filterType;
        }

        public virtual async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress ?? new IPAddress(new byte[0]);
            Logger.LogInformation("Remote IpAddress: {RemoteIp}", remoteIp);
            bool isIPFilter=Convert.ToBoolean(AppConfig.configuration.GetSection($"Modules:SystemConfig")["IPFilterIsOn"].ToString());
            var ipFilterValues = AppConfig.configuration?.GetSection($"Modules:SystemConfig")["IPFilter"]?.Split(';') ?? Array.Empty<string>();

            var badIp = true;
            if (isIPFilter)
            {

                if (remoteIp.IsIPv4MappedToIPv6)
                {
                    remoteIp = remoteIp.MapToIPv4();
                }

                foreach (var address in ipFilterValues)
                {
                    var testIp = IPAddress.Parse(address);

                    if (testIp.Equals(remoteIp))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    //Logger.LogWarning("Forbidden Request from IP: {RemoteIp}", remoteIp);
                    context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.HttpContext.Response.WriteAsync($"Forbidden Request from IP: {remoteIp}");
                    await context.HttpContext.Response.CompleteAsync();
                    return context;
                }
            }
            return await next(context);
        }
    }

    class LocalhostIPFilter : IPFilter
    {
        public LocalhostIPFilter(ILoggerFactory loggerFactory) : base(loggerFactory, "Localhost") { }
    }
    
}
