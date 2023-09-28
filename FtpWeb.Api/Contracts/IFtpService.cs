using FtpWeb.Shared.Models;
using FtpWeb.Shared.Models.Result;

namespace FtpWeb.Api.Contracts;

public interface IFtpService : IDisposable
{
    Task<ResultModel<IEnumerable<FtpItemModel>>> DiscoverAsync(string? path, CancellationToken cancellationToken = default);
    Task<ResultModel<string>> UploadFileAsync(string path, Stream stream, CancellationToken cancellationToken = default);
    Task<ResultModel<Stream>> DownloadFileAsync(string path, CancellationToken cancellationToken = default);
    Task<ResultModel<string>> DeleteDirectoryAsync(string path, CancellationToken cancellationToken = default);
    Task<ResultModel<string>> DeleteFileAsync(string path, CancellationToken cancellationToken = default);
    Task<ResultModel<string>> RenameAsync(FtpRenameModel model, CancellationToken cancellationToken = default);
    Task<Stream> GetStreamAsync(string path, CancellationToken cancellationToken = default);
}
