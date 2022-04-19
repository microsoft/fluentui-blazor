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
        services.AddTransient<AccentFillRestDelta>();
        services.AddTransient<AccentFillHoverDelta>();
        services.AddTransient<AccentFillActiveDelta>();
        services.AddTransient<AccentFillFocusDelta>();
        services.AddTransient<AccentForegroundRestDelta>();
        services.AddTransient<AccentForegroundHoverDelta>();
        services.AddTransient<AccentForegroundActiveDelta>();
        services.AddTransient<AccentForegroundFocusDelta>();
        services.AddTransient<NeutralFillRestDelta>();
        services.AddTransient<NeutralFillHoverDelta>();
        services.AddTransient<NeutralFillActiveDelta>();
        services.AddTransient<NeutralFillFocusDelta>();
        services.AddTransient<NeutralFillInputRestDelta>();
        services.AddTransient<NeutralFillInputHoverDelta>();
        services.AddTransient<NeutralFillInputActiveDelta>();
        services.AddTransient<NeutralFillInputFocusDelta>();
        services.AddTransient<NeutralFillStealthRestDelta>();
        services.AddTransient<NeutralFillStealthHoverDelta>();
        services.AddTransient<NeutralFillStealthActiveDelta>();
        services.AddTransient<NeutralFillStealthFocusDelta>();
        services.AddTransient<NeutralFillStrongRestDelta>();
        services.AddTransient<NeutralFillStrongHoverDelta>();
        services.AddTransient<NeutralFillStrongActiveDelta>();
        services.AddTransient<NeutralFillStrongFocusDelta>();
        services.AddTransient<NeutralFillLayerRestDelta>();
        services.AddTransient<NeutralStrokeRestDelta>();
        services.AddTransient<NeutralStrokeHoverDelta>();
        services.AddTransient<NeutralStrokeActiveDelta>();
        services.AddTransient<NeutralStrokeFocusDelta>();
        services.AddTransient<NeutralStrokeDividerRestDelta>();
    }
}