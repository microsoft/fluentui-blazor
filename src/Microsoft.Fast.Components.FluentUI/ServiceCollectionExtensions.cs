using Microsoft.Extensions.DependencyInjection;
using Microsoft.Fast.Components.FluentUI.DesignTokens;

namespace Microsoft.Fast.Components.FluentUI;

public static class ServiceCollectionExtensions
{
    public static void AddFluentUIComponents(this IServiceCollection services)
    {
        services.AddScoped<IconService>();

        services.AddTransient<BodyFont>();
        services.AddTransient<BaseHeightMultiplier>();
        services.AddTransient<BaseHorizontalSpacingMultiplier>();
        services.AddTransient<BaseLayerLuminance>();
        services.AddTransient<ControlCornerRadius>();
        services.AddTransient<Density>();
        services.AddTransient<DesignUnit>();
        services.AddTransient<DesignTokens.Direction>();
        services.AddTransient<DisabledOpacity>();
        services.AddTransient<StrokeWidth>();
        services.AddTransient<FillColor>();
        services.AddTransient<FocusStrokeWidth>();
        services.AddTransient<TypeRampBaseFontSize>();
        services.AddTransient<TypeRampBaseLineHeight>();
        services.AddTransient<TypeRampMinus1FontSize>();
        services.AddTransient<TypeRampMinus1LineHeight>();
        services.AddTransient<TypeRampMinus2FontSize>();
        services.AddTransient<TypeRampMinus2LineHeight>();
        services.AddTransient<TypeRampPlus1FontSize>();
        services.AddTransient<TypeRampPlus1LineHeight>();
        services.AddTransient<TypeRampPlus2FontSize>();
        services.AddTransient<TypeRampPlus2LineHeight>();
        services.AddTransient<TypeRampPlus3FontSize>();
        services.AddTransient<TypeRampPlus3LineHeight>();
        services.AddTransient<TypeRampPlus4FontSize>();
        services.AddTransient<TypeRampPlus4LineHeight>();
        services.AddTransient<TypeRampPlus5FontSize>();
        services.AddTransient<TypeRampPlus5LineHeight>();
        services.AddTransient<TypeRampPlus6FontSize>();
        services.AddTransient<TypeRampPlus6LineHeight>();
        services.AddTransient<AccentFillRest>();
        services.AddTransient<AccentFillHover>();
        services.AddTransient<AccentFillActive>();
        services.AddTransient<AccentFillFocus>();
        services.AddTransient<AccentForegroundRest>();
        services.AddTransient<AccentForegroundHover>();
        services.AddTransient<AccentForegroundActive>();
        services.AddTransient<AccentForegroundFocus>();
        services.AddTransient<NeutralFillRest>();
        services.AddTransient<NeutralFillHover>();
        services.AddTransient<NeutralFillActive>();
        services.AddTransient<NeutralFillFocus>();
        services.AddTransient<NeutralFillInputRest>();
        services.AddTransient<NeutralFillInputHover>();
        services.AddTransient<NeutralFillInputActive>();
        services.AddTransient<NeutralFillInputFocus>();
        services.AddTransient<NeutralFillStealthRest>();
        services.AddTransient<NeutralFillStealthHover>();
        services.AddTransient<NeutralFillStealthActive>();
        services.AddTransient<NeutralFillStealthFocus>();
        services.AddTransient<NeutralFillStrongRest>();
        services.AddTransient<NeutralFillStrongHover>();
        services.AddTransient<NeutralFillStrongActive>();
        services.AddTransient<NeutralFillStrongFocus>();
        services.AddTransient<NeutralFillLayerRest>();
        services.AddTransient<NeutralStrokeRest>();
        services.AddTransient<NeutralStrokeHover>();
        services.AddTransient<NeutralStrokeActive>();
        services.AddTransient<NeutralStrokeFocus>();
        services.AddTransient<NeutralStrokeDividerRest>();
    }
}