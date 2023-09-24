using FluentFTP;
using FtpWeb.Shared.Models;

namespace FtpWeb.Api.Extensions;

internal static class FtpExtensions
{
    public static FtpItemModel ToItemModel(this FtpListItem item)
    {
        return new(item.Name, item.FullName, item.Type == FtpObjectType.Directory);
    }
}
