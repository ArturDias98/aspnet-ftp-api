namespace FtpWeb.Shared.Models;

public class FtpItemModel
{
    public FtpItemModel() { }

    public FtpItemModel(string name, string fullName, bool isFolder)
    {
        Name = name;
        FullName = fullName;
        IsFolder = isFolder;
    }

    public string Name { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsFolder { get; set; }
}
