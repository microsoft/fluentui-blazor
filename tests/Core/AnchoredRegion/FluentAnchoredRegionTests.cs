using Bunit;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.AnchoredRegion;

public class FluentAnchoredRegionTests : TestBase
{
    [Fact]
    public void FluentAnchoredRegion_AttributeDefaultValues()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_AnchorAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.Anchor, "anchor-id");
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_ViewportAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.Viewport, "viewport-value");
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(AxisPositioningMode.Dynamic)]
    [InlineData(AxisPositioningMode.Locktodefault)]
    [InlineData(AxisPositioningMode.Uncontrolled)]
    public void FluentAnchoredRegion_HorizontalPositioningAttribute(AxisPositioningMode axisPositioningMode)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.HorizontalPositioningMode, axisPositioningMode);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: axisPositioningMode.ToString());
    }

    [Theory]
    [InlineData(HorizontalPosition.End)]
    [InlineData(HorizontalPosition.Left)]
    [InlineData(HorizontalPosition.Right)]
    [InlineData(HorizontalPosition.Start)]
    [InlineData(HorizontalPosition.Unset)]
    public void FluentAnchoredRegion_HorizontalDefaultPositionAttribute(HorizontalPosition horizontalPosition)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.HorizontalDefaultPosition, horizontalPosition);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: horizontalPosition.ToString());
    }

    [Fact]
    public void FluentAnchoredRegion_HorizontalViewportLockAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.HorizontalViewportLock, true);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_HorizontalInsetAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.HorizontalInset, true);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_HorizontalThresholdAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.HorizontalThreshold, 10);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(AxisScalingMode.Anchor)]
    [InlineData(AxisScalingMode.Content)]
    [InlineData(AxisScalingMode.Fill)]
    public void FluentAnchoredRegion_HorizontalScalingAttribute(AxisScalingMode axisScalingMode)
    {
        // Arrange && Act

        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.HorizontalScaling, axisScalingMode);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: axisScalingMode.ToString());
    }

    [Theory]
    [InlineData(AxisPositioningMode.Dynamic)]
    [InlineData(AxisPositioningMode.Locktodefault)]
    [InlineData(AxisPositioningMode.Uncontrolled)]
    public void FluentAnchoredRegion_VerticalPositioningModeAttribute(AxisPositioningMode axisPositioningMode)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.VerticalPositioningMode, axisPositioningMode);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: axisPositioningMode.ToString());
    }

    [Theory]
    [InlineData(VerticalPosition.Unset)]
    [InlineData(VerticalPosition.Bottom)]
    [InlineData(VerticalPosition.Top)]
    public void FluentAnchoredRegion_VerticalDefaultPositionAttribute(VerticalPosition verticalPosition)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.VerticalDefaultPosition, verticalPosition);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: verticalPosition.ToString());
    }

    [Fact]
    public void FluentAnchoredRegion_VerticalViewportLockAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.VerticalViewportLock, true);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_VerticalInsetAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.VerticalInset, true);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_VerticalThresholdAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.VerticalThreshold, 100);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(AxisScalingMode.Content)]
    [InlineData(AxisScalingMode.Anchor)]
    [InlineData(AxisScalingMode.Fill)]
    public void FluentAnchoredRegion_VerticalScalingAttribute(AxisScalingMode axisScalingMode)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.VerticalScaling, axisScalingMode);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: axisScalingMode.ToString());
    }

    [Fact]
    public void FluentAnchoredRegion_FixedPlacementAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.FixedPlacement, true);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Theory]
    [InlineData(AutoUpdateMode.Auto)]
    [InlineData(AutoUpdateMode.Anchor)]
    public void FluentAnchoredRegion_AutoUpdateModeAttribute(AutoUpdateMode autoUpdateMode)
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.AutoUpdateMode, autoUpdateMode);
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify(suffix: autoUpdateMode.ToString());
    }

    [Fact]
    public void FluentAnchoredRegion_WithAdditionalCSSClass()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.Class, "additional-css-class");
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_WithAdditionalStyle()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.Add(p => p.Style, "background-color: black");
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_WithAnAdditionalAttribute()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.AddUnmatched("additional", "additional-value");
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }

    [Fact]
    public void FluentAnchoredRegion_WithMultipleAdditionalAttributes()
    {
        // Arrange && Act
        var cut = TestContext.RenderComponent<FluentAnchoredRegion>(parameters =>
        {
            parameters.AddUnmatched("additional1", "additional1-value");
            parameters.AddUnmatched("additional2", "additional2-value");
            parameters.AddChildContent("content");
        });

        // Assert
        cut.Verify();
    }
}
