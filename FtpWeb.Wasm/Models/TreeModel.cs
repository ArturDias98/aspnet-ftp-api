using FtpWeb.Shared.Models;
using MudBlazor;

namespace FtpWeb.Wasm.Models;

public record TreeModel(string Name, string FullName, bool IsFolder)
    : FtpItemModel(Name, FullName, IsFolder)
{
    public HashSet<TreeModel> Items { get; set; } = new();
    public string Icon => IsFolder
        ? Icons.Material.Filled.Folder
        : Icons.Custom.FileFormats.FileDocument;
    public bool CanExpand => Items.Count > 0;
}
