using ClaimsPay.Filters;

namespace ClaimsPay.Modules.ClaimsPay.ClaimsPayFilter
{
    public class ClaimsPayFilter : AuthenticationFilter
    {
        public ClaimsPayFilter(ILoggerFactory loggerFactory) : base(loggerFactory, "ClaimsPay") { }
        
    }
}
