using Microsoft.Extensions.Options;

namespace FtpWeb.Api.Options.Ftp;

internal class FtpOptionsSetup : IConfigureOptions<FtpOptions>
{
    private readonly IConfiguration _configuration;

    public FtpOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(FtpOptions options)
    {
        _configuration
            .GetSection(FtpOptions.Section)
            .Bind(options);
    }
}
