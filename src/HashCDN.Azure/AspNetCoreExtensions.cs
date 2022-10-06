using HashCDN;
using HashCDN.Azure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.Configuration
{
    public static class AspNetCoreExtensions
    {
        public static void AddHashCDNAzure(this IServiceCollection services, IConfigurationSection configSection)
        {
            services.Configure<AzureStorageOptions>(configSection);

            services.AddSingleton<IFileStorer, FileStorer>();

            services.AddHashCDN();
        }
    }
}
