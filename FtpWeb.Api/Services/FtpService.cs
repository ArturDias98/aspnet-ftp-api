using FluentFTP;
using FtpWeb.Api.Contracts;
using FtpWeb.Api.Extensions;
using FtpWeb.Api.Options.Ftp;
using FtpWeb.Shared.Models;
using FtpWeb.Shared.Models.Result;
using Microsoft.Extensions.Options;
using System.Text;

namespace FtpWeb.Api.Services;

internal class FtpService : IFtpService
{
    private readonly FtpOptions _options;
    private readonly ILogger<FtpService> _logger;
    private readonly AsyncFtpClient _client;

    public FtpService(IOptions<FtpOptions> options, ILogger<FtpService> logger)
    {
        _options = options.Value;
        _logger = logger;

        _client = new AsyncFtpClient(
            _options.Host,
            _options.User,
            _options.Password);
    }

    private async Task EnsureConnection(CancellationToken cancellationToken)
    {
        if (_client.IsConnected)
        {
            return;
        }

        await _client.Connect(cancellationToken);

        await _client.SetWorkingDirectory(_options.WorkDir, cancellationToken);
    }

    public async Task<ResultModel<IEnumerable<FtpItemModel>>> DiscoverAsync(string? path, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnection(cancellationToken);

            FtpListItem[] items = await _client.GetListing(path, cancellationToken);

            return new(items.Select(i => i.ToItemModel()));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error on discover items", ex.Message);
            return new(false, "Error on discover directory/item");
        }
    }

    public async Task<ResultModel<Stream>> DownloadFileAsync(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnection(cancellationToken);

            await _client.Connect(cancellationToken);

            await _client.SetWorkingDirectory(_options.WorkDir, cancellationToken);

            return new(await _client.OpenRead(path, token: cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogError("Error on download file", ex.Message);
            return new(false, "Error on open stream");
        }
    }

    public async Task<ResultModel<string>> UploadFileAsync(string path, Stream stream, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnection(cancellationToken);

            await _client.Connect(cancellationToken);

            await _client.SetWorkingDirectory(_options.WorkDir, cancellationToken);

            var status = await _client.UploadStream(
                stream,
                path,
                FtpRemoteExists.Overwrite,
                true,
                token: cancellationToken);

            var builder = new StringBuilder();

            if (status == FtpStatus.Success)
            {
                return new(builder
                    .Append("File uploaded successfully. ")
                    .Append("Remote path: ")
                    .Append(path)
                    .ToString());
            }

            return new(false, "Failed on upload file");
        }
        catch (Exception ex)
        {
            _logger.LogError("Error on upload file", ex.Message);
            return new(false, "Error on upload file");
        }
    }

    public async Task<ResultModel<string>> DeleteDirectoryAsync(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnection(cancellationToken);

            await _client.DeleteDirectory(path, cancellationToken);

            return new(path);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error on delete directory", ex.Message);
            return new(false, "Error on delete directory");
        }
    }

    public async Task<ResultModel<string>> DeleteFileAsync(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnection(cancellationToken);

            await _client.DeleteFile(path, cancellationToken);

            return new(path);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error on delete file", ex.Message);
            return new(false, "Error on delete file");
        }
    }

    public async Task<ResultModel<string>> RenameAsync(FtpRenameModel model, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureConnection(cancellationToken);

            await _client.Rename(model.OldPath, model.NewPath, cancellationToken);

            return new(model.NewPath);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error on delete file", ex.Message);
            return new(false, "Error on rename directory/item");
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
