using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DesignSystemProvider;
public class FluentDesignSystemProviderTests : TestBase
{
    [Fact]
    public void FluentDesignSystemProvider_Default()
    {
        //Arrange
        var childContent = "<b>render me</b>";
        bool? noPaint = default!;
        string fillColor = default!;
        string accentBaseColor = default!;
        string neutralBaseColor = default!;
        int? density = default!;
        int? designUnit = default!;
        LocalizationDirection? direction = default!;
        int? baseHeightMultiplier = default!;
        int? baseHorizontalSpacingMultiplier = default!;
        int? controlCornerRadius = default!;
        int? layerCornerRadius = default!;
        int? strokeWidth = default!;
        int? focusStrokeWidth = default!;
        float? disabledOpacity = default!;
        string typeRampMinus2FontSize = default!;
        string typeRampMinus2LineHeight = default!;
        string typeRampMinus1FontSize = default!;
        string typeRampMinus1LineHeight = default!;
        string typeRampBaseFontSize = default!;
        string typeRampBaseLineHeight = default!;
        string typeRampPlus1FontSize = default!;
        string typeRampPlus1LineHeight = default!;
        string typeRampPlus2FontSize = default!;
        string typeRampPlus2LineHeight = default!;
        string typeRampPlus3FontSize = default!;
        string typeRampPlus3LineHeight = default!;
        string typeRampPlus4FontSize = default!;
        string typeRampPlus4LineHeight = default!;
        string typeRampPlus5FontSize = default!;
        string typeRampPlus5LineHeight = default!;
        string typeRampPlus6FontSize = default!;
        string typeRampPlus6LineHeight = default!;
        int? accentFillRestDelta = default!;
        int? accentFillHoverDelta = default!;
        int? accentFillActiveDelta = default!;
        int? accentFillFocusDelta = default!;
        int? accentForegroundRestDelta = default!;
        int? accentForegroundHoverDelta = default!;
        int? accentForegroundActiveDelta = default!;
        int? accentForegroundFocusDelta = default!;
        int? neutralFillRestDelta = default!;
        int? neutralFillHoverDelta = default!;
        int? neutralFillActiveDelta = default!;
        int? neutralFillFocusDelta = default!;
        int? neutralFillInputRestDelta = default!;
        int? neutralFillInputHoverDelta = default!;
        int? neutralFillInputActiveDelta = default!;
        int? neutralFillInputFocusDelta = default!;
        int? neutralFillLayerRestDelta = default!;
        int? neutralFillStealthRestDelta = default!;
        int? neutralFillStealthHoverDelta = default!;
        int? neutralFillStealthActiveDelta = default!;
        int? neutralFillStealthFocusDelta = default!;
        int? neutralFillStrongHoverDelta = default!;
        int? neutralFillStrongActiveDelta = default!;
        int? neutralFillStrongFocusDelta = default!;
        int? neutralStrokeDividerRestDelta = default!;
        int? neutralStrokeRestDelta = default!;
        int? neutralStrokeHoverDelta = default!;
        int? neutralStrokeActiveDelta = default!;
        int? neutralStrokeFocusDelta = default!;
        float? baseLayerLuminance = default!;
        var cut = TestContext.RenderComponent<FluentDesignSystemProvider>(parameters => parameters
            .Add(p => p.NoPaint, noPaint)
            .Add(p => p.FillColor, fillColor)
            .Add(p => p.AccentBaseColor, accentBaseColor)
            .Add(p => p.NeutralBaseColor, neutralBaseColor)
            .Add(p => p.Density, density)
            .Add(p => p.DesignUnit, designUnit)
            .Add(p => p.Direction, direction)
            .Add(p => p.BaseHeightMultiplier, baseHeightMultiplier)
            .Add(p => p.BaseHorizontalSpacingMultiplier, baseHorizontalSpacingMultiplier)
            .Add(p => p.ControlCornerRadius, controlCornerRadius)
            .Add(p => p.LayerCornerRadius, layerCornerRadius)
            .Add(p => p.StrokeWidth, strokeWidth)
            .Add(p => p.FocusStrokeWidth, focusStrokeWidth)
            .Add(p => p.DisabledOpacity, disabledOpacity)
            .Add(p => p.TypeRampMinus2FontSize, typeRampMinus2FontSize)
            .Add(p => p.TypeRampMinus2LineHeight, typeRampMinus2LineHeight)
            .Add(p => p.TypeRampMinus1FontSize, typeRampMinus1FontSize)
            .Add(p => p.TypeRampMinus1LineHeight, typeRampMinus1LineHeight)
            .Add(p => p.TypeRampBaseFontSize, typeRampBaseFontSize)
            .Add(p => p.TypeRampBaseLineHeight, typeRampBaseLineHeight)
            .Add(p => p.TypeRampPlus1FontSize, typeRampPlus1FontSize)
            .Add(p => p.TypeRampPlus1LineHeight, typeRampPlus1LineHeight)
            .Add(p => p.TypeRampPlus2FontSize, typeRampPlus2FontSize)
            .Add(p => p.TypeRampPlus2LineHeight, typeRampPlus2LineHeight)
            .Add(p => p.TypeRampPlus3FontSize, typeRampPlus3FontSize)
            .Add(p => p.TypeRampPlus3LineHeight, typeRampPlus3LineHeight)
            .Add(p => p.TypeRampPlus4FontSize, typeRampPlus4FontSize)
            .Add(p => p.TypeRampPlus4LineHeight, typeRampPlus4LineHeight)
            .Add(p => p.TypeRampPlus5FontSize, typeRampPlus5FontSize)
            .Add(p => p.TypeRampPlus5LineHeight, typeRampPlus5LineHeight)
            .Add(p => p.TypeRampPlus6FontSize, typeRampPlus6FontSize)
            .Add(p => p.TypeRampPlus6LineHeight, typeRampPlus6LineHeight)
            .Add(p => p.AccentFillRestDelta, accentFillRestDelta)
            .Add(p => p.AccentFillHoverDelta, accentFillHoverDelta)
            .Add(p => p.AccentFillActiveDelta, accentFillActiveDelta)
            .Add(p => p.AccentFillFocusDelta, accentFillFocusDelta)
            .Add(p => p.AccentForegroundRestDelta, accentForegroundRestDelta)
            .Add(p => p.AccentForegroundHoverDelta, accentForegroundHoverDelta)
            .Add(p => p.AccentForegroundActiveDelta, accentForegroundActiveDelta)
            .Add(p => p.AccentForegroundFocusDelta, accentForegroundFocusDelta)
            .Add(p => p.NeutralFillRestDelta, neutralFillRestDelta)
            .Add(p => p.NeutralFillHoverDelta, neutralFillHoverDelta)
            .Add(p => p.NeutralFillActiveDelta, neutralFillActiveDelta)
            .Add(p => p.NeutralFillFocusDelta, neutralFillFocusDelta)
            .Add(p => p.NeutralFillInputRestDelta, neutralFillInputRestDelta)
            .Add(p => p.NeutralFillInputHoverDelta, neutralFillInputHoverDelta)
            .Add(p => p.NeutralFillInputActiveDelta, neutralFillInputActiveDelta)
            .Add(p => p.NeutralFillInputFocusDelta, neutralFillInputFocusDelta)
            .Add(p => p.NeutralFillLayerRestDelta, neutralFillLayerRestDelta)
            .Add(p => p.NeutralFillStealthRestDelta, neutralFillStealthRestDelta)
            .Add(p => p.NeutralFillStealthHoverDelta, neutralFillStealthHoverDelta)
            .Add(p => p.NeutralFillStealthActiveDelta, neutralFillStealthActiveDelta)
            .Add(p => p.NeutralFillStealthFocusDelta, neutralFillStealthFocusDelta)
            .Add(p => p.NeutralFillStrongHoverDelta, neutralFillStrongHoverDelta)
            .Add(p => p.NeutralFillStrongActiveDelta, neutralFillStrongActiveDelta)
            .Add(p => p.NeutralFillStrongFocusDelta, neutralFillStrongFocusDelta)
            .Add(p => p.NeutralStrokeDividerRestDelta, neutralStrokeDividerRestDelta)
            .Add(p => p.NeutralStrokeRestDelta, neutralStrokeRestDelta)
            .Add(p => p.NeutralStrokeHoverDelta, neutralStrokeHoverDelta)
            .Add(p => p.NeutralStrokeActiveDelta, neutralStrokeActiveDelta)
            .Add(p => p.NeutralStrokeFocusDelta, neutralStrokeFocusDelta)
            .Add(p => p.BaseLayerLuminance, baseLayerLuminance)
            .AddChildContent(childContent)
        );
        //Act

        //Assert
        cut.Verify();
    }
}

