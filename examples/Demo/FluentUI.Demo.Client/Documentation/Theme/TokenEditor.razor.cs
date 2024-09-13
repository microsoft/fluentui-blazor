// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FluentUI.Demo.Client.Documentation.Theme;

public partial class TokenEditor
{
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [Inject]
    protected virtual IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public string SearchText { get; set; } = string.Empty;

    private IDictionary<string, TokenItem> Tokens { get; set; } = new Dictionary<string, TokenItem>();

    public IDictionary<string, object> GetTheme()
    {
        return Tokens.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
    }

    public async Task ApplyThemeAsync()
    {
        await JSRuntime.InvokeAsync<JsonElement>("Blazor.theme.update", GetTheme());
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var tokensJson = await JSRuntime.InvokeAsync<JsonElement>("Blazor.theme.getAllTokensValues");
            Tokens = JsonSerializer.Deserialize<Dictionary<string, TokenItem>>(tokensJson.GetRawText(), JsonOptions) ?? [];

            foreach (var token in Tokens)
            {
                token.Value.Name = token.Key;
            }

            StateHasChanged();
        }
    }

    private IEnumerable<string> GetSections(string mainSection = "")
    {
        var tokenFilter = Tokens.Where(i => i.Key.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                            i.Value.ValueAsString.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        if (string.IsNullOrEmpty(mainSection))
        {
            return tokenFilter.Select(i => i.Value.MainSection).Distinct().Order();
        }

        return tokenFilter.Where(i => i.Value.MainSection == mainSection).Select(i => i.Value.SubSection).Distinct().Order();
    }

    private static string GetInputType(TokenItem token)
    {
        if (token.Type == "number")
        {
            return "number";
        }

        if (token.MainSection == "Color")
        {
            return "color";
        }

        return "text";
    }
}
