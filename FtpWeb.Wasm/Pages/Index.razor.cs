using FtpWeb.Wasm.Models;

namespace FtpWeb.Wasm.Pages;

public partial class Index
{
    private HashSet<TreeModel> _treeItems = new();

    private async Task<HashSet<TreeModel>> LoadItems(TreeModel item)
    {
        return item.Items;
    }

    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }
}
