using AddressNormalizer.Application;
using Microsoft.Extensions.DependencyInjection;

namespace AddressNormalizer.Infrastructure;

public static class ServicesExtension
{
    public static IServiceCollection AddAddressNormalizedServices(this IServiceCollection services)
    {
        services.AddSingleton<NormalizerLibpostal>();
        services.AddSingleton<ILibpostalLoader>(provider=> provider.GetService<NormalizerLibpostal>()!);
        services.AddSingleton<INormalizerLibpostal>(provider => provider.GetService<NormalizerLibpostal>()!);
        return services;
    }
}