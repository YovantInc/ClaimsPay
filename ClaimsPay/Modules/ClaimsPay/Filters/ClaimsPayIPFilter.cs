using ClaimsPay.Filters;

namespace ClaimsPay.Modules.ClaimsPay.Filters
{
    
        class ClaimsPayIPFilter : IPFilter
        {
            public ClaimsPayIPFilter(ILoggerFactory loggerFactory) : base(loggerFactory, "ClaimsPay") { }
        }
    
}
