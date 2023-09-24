namespace FtpWeb.Shared.Models.Result;

public abstract class ResultBase
{
    public bool Success { get; set; }
    public IReadOnlyCollection<string> Errors { get; set; } = new List<string>();
}
