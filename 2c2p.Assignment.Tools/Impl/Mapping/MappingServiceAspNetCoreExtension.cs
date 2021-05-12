using _2c2p.Assignment.Tools.Abstraction.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace _2c2p.Assignment.Tools.Impl.Mapping
{
    public static class MappingServiceAspNetCoreExtension
    {
        public static void AddMappingService(this IServiceCollection services, params IMappingConfiguration[] mappingConfigurations)
        {
            services.AddTransient<IMappingServiceProvider>((a) =>
            {
                var mappingService = new MappingService(mappingConfigurations);
                return mappingService;
            });
        }
    }
}
