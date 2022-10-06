using HashCDN;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Configuration
{
    public static class AspNetCoreExtensions
    {
        public static void AddHashCDN(this IServiceCollection services)
        {
            services.AddSingleton<CDN>();
        }

        public static void UseHashCDN(this IApplicationBuilder app)
        {
            //app.UseMiddleware<MyMiddleware>();
        }
    }
}
