namespace FtpWeb.Api.Options.Ftp;

internal class FtpOptions
{
    public const string Section = "Ftp";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string WorkDir { get; set; } = string.Empty;
}
