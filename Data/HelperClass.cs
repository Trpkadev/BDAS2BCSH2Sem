namespace BCSH2BDAS2.Data;

/// <summary>Holds strings/constants for the rest of the code</summary>
/// <remarks>"Antipattern" Singleton, in our case somewhat useless, might change to static class + static string(int...) / resources :)</remarks>
internal class StrCls
{
    private static readonly Lazy<StrCls> _instance = new(() => new StrCls());
    public static StrCls Instance => _instance.Value;

    internal string TitleCreate { get; } = "Create";
    internal string TitleEdit { get; } = "Edit";
    internal string TitleIndex { get; } = "Index";
    internal string TitleDelete { get; } = "Delete";
    internal string TitleDetails { get; } = "Details";
}

/// <summary>
/// Model for page initialization
/// </summary>
/// <param name="titleName">Name to show in HTML Title</param>
/// <param name="model">Model to return</param>
internal class ViewInitModel(string titleName, object? model = null)
{
    public string TitleName { get; } = titleName;
    public object? Model { get; } = model;
}