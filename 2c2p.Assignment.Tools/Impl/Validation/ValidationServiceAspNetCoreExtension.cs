using _2c2p.Assignment.Tools.Abstraction.Mapping;
using _2c2p.Assignment.Tools.Abstraction.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace _2c2p.Assignment.Tools.Impl.Validation
{
    public static class MappingServiceAspNetCoreExtension
    {
        public static void AddValidationService(this IServiceCollection services, params IValidationConfiguration[] mappingConfigurations)
        {
            services.AddTransient<IValidationServiceProvider>((a) =>
            {
                var validationService = new ValidationService(mappingConfigurations);
                return validationService;
            });
        }
    }
}
