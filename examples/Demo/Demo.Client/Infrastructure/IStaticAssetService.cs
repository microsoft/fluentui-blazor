namespace FluentUI.Demo.Client;

public interface IStaticAssetService
{
    public Task<string?> GetAsync(string assetUrl, bool useCache = true);
}
