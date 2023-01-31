namespace Microsoft.Fast.Components.FluentUI.Infrastructure;

public interface IStaticAssetService
{
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
