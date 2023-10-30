namespace Microsoft.FluentUI.AspNetCore.Components.Infrastructure;

public interface IStaticAssetService
{
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
