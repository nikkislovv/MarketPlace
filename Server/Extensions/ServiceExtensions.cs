using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
       services.Configure<IISOptions>(options =>
       {

       });
    }
}
