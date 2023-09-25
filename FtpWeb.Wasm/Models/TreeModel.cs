using FtpWeb.Shared.Models;
using MudBlazor;

namespace FtpWeb.Wasm.Models;

public class TreeModel : FtpItemModel
{
    public TreeModel(string name, string fullName, bool isFolder)
        : base(name, fullName, isFolder)
    {
    }

    public HashSet<TreeModel> Items { get; set; } = new();
    public string Icon => IsFolder
        ? Icons.Material.Filled.Folder
        : Icons.Custom.FileFormats.FileDocument;
    public bool CanExpand => IsFolder;

    public string Url => Uri.EscapeDataString(FullName);
}
