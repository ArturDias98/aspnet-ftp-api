using FtpWeb.Api.Contracts;
using FtpWeb.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;

namespace FtpWeb.Api.Extensions;

internal static class MapEndpoints
{
    public static WebApplication AddEndpoints(this WebApplication app)
    {
        return app
            .AddPost()
            .AddGet()
            .AddDelete()
            .AddPut();
    }

    private static WebApplication AddPost(this WebApplication app)
    {
        app.MapPost("api/v1/{path}", async (
            IFormFile file,
            [FromRoute] string path,
            [FromServices] IFtpService service,
            CancellationToken token) =>
        {
            var parse = HttpUtility.UrlDecode(path);

            var result = await service.UploadFileAsync(
                parse,
                file.OpenReadStream(),
                token);

            return Results.Json(result);
        }).WithName("Upload");

        return app;
    }

    private static WebApplication AddGet(this WebApplication app)
    {
        app.MapGet("api/v1/{path}", async (
            [FromRoute] string? path,
            [FromServices] IFtpService service,
            CancellationToken token) =>
        {
            var decode = HttpUtility.UrlDecode(path);
            var list = await service.DiscoverAsync(decode, token);

            return Results.Json(list);
        }).WithName("List");

        app.MapGet("api/v1/download/{path}", async (
            [FromRoute] string path,
            [FromServices] IFtpService service,
            CancellationToken token) =>
        {
            var parse = HttpUtility.UrlDecode(path);
            var filename = Path.GetFileName(parse);

            var result = await service.DownloadFileAsync(parse, token);

            if (result.Success)
            {
                return Results.File(result.Result!, "application/octet-stream", filename);
            }

            return Results.Json(result);
        });

        return app;
    }

    private static WebApplication AddDelete(this WebApplication app)
    {
        app.MapDelete("api/v1/directory/{path}", async (
            [FromRoute] string path,
            [FromServices] IFtpService service,
            CancellationToken token) =>
        {
            var parse = HttpUtility.UrlDecode(path);

            var result = await service.DeleteDirectoryAsync(parse, token);

            return result.Success
            ? Results.Ok(result)
            : Results.BadRequest(result);
        });

        app.MapDelete("api/v1/file/{path}", async (
            [FromRoute] string path,
            [FromServices] IFtpService service,
            CancellationToken token) =>
        {
            var parse = HttpUtility.UrlDecode(path);

            var result = await service.DeleteFileAsync(parse, token);

            return result.Success
            ? Results.Ok(result)
            : Results.BadRequest(result);
        });

        return app;
    }

    private static WebApplication AddPut(this WebApplication app)
    {
        app.MapPut("api/v1/rename", async (
            [FromBody] FtpRenameModel model,
            [FromServices] IFtpService service,
            CancellationToken token) =>
        {
            var result = await service.RenameAsync(model, token);

            return result.Success
            ? Results.Ok(result)
            : Results.BadRequest(result);
        });

        return app;
    }
}
