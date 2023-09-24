using FtpWeb.Api.Contracts;
using FtpWeb.Api.Options.Ftp;
using FtpWeb.Api.Services;

namespace FtpWeb.Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        return services
            .AddConfigurations()
            .AddTransient<IFtpService, FtpService>();
    }

    private static IServiceCollection AddConfigurations(this IServiceCollection services)
    {
        return services
            .ConfigureOptions<FtpOptionsSetup>();
    }
}
