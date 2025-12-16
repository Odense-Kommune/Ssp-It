using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Dk.Odense.SSP.Web.Auth
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly bool isDevelopment;
        private readonly bool isAuthenticated = true;


        public HangfireAuthorizationFilter(IWebHostEnvironment isDevelopment)
        {
            this.isDevelopment = isDevelopment.IsDevelopment();

            //isAuthenticated = contextAccessor.HttpContext?.User != null && contextAccessor.HttpContext.User.Identity.IsAuthenticated;

        }

        public bool Authorize(DashboardContext context)
        {
            //if (isDevelopment) return true;
            
            return isAuthenticated;
        }
    }
}
