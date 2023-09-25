using FtpWeb.Shared.Models;
using FtpWeb.Shared.Models.Result;
using FtpWeb.Wasm.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;

namespace FtpWeb.Wasm.Pages;

public partial class Index
{
    private HashSet<TreeModel> _treeItems = new();

    private async Task<HashSet<TreeModel>> LoadItems(TreeModel item)
    {
        var load = await RequestItems(item.FullName);

        foreach (var nodeItem in load)
        {
            item.Items.Add(nodeItem);
        }

        return item.Items;
    }

    private async Task<IEnumerable<TreeModel>> RequestItems(string node)
    {
        try
        {
            var encode = Uri.EscapeDataString(node);
            var result = await Client
           .GetFromJsonAsync<ResultModel<IEnumerable<FtpItemModel>>>($"api/v1/{encode}");

            if (result == null)
            {
                return Enumerable.Empty<TreeModel>();
            }

            return result.Result?
                .Select(i => new TreeModel(i.Name, i.FullName, i.IsFolder))
                ?? Enumerable.Empty<TreeModel>();
        }
        catch (Exception)
        {
             return Enumerable.Empty<TreeModel>();
        }
    }

    private async Task OnFileChanged(IBrowserFile file)
    {
        try
        {
            using var form = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenReadStream(30000000));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(fileContent, "file", file.Name);

            var parse = Uri.EscapeDataString($"files/{file.Name}");
            using var response = await Client.PostAsync($"api/v1/{parse}", form);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {

        }
    }

    [Inject] HttpClient Client { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var request = await RequestItems(" ");
        foreach (var item in request)
        {
            _treeItems.Add(item);
        }
    }
}
