using Bunit;
using Xunit;

namespace Microsoft.Fast.Components.FluentUI.Tests.AnchoredRegion
{
    public class FluentAnchoredRegionShould : TestBase
    {
        [Fact]
        public void RenderProperly_AttributeDefaultValues()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters.AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }
        
        [Fact]
        public void RenderProperly_AnchorAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.Anchor, "anchor-id")
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "anchor=\"anchor-id\" " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_ViewportAttribute()
        {
            // Arrange && Act
            string content = "content";
            string viewPortValue = "viewport-value";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.Viewport, viewPortValue)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              $"viewport=\"{viewPortValue}\" " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(AxisPositioningMode.Dynamic)]
        [InlineData(AxisPositioningMode.Locktodefault)]
        [InlineData(AxisPositioningMode.Uncontrolled)]
        public void RenderProperly_HorizontalPositioningAttribute(AxisPositioningMode axisPositioningMode)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.HorizontalPositioningMode, axisPositioningMode)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              $"horizontal-positioning-mode=\"{axisPositioningMode.ToAttributeValue()}\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(HorizontalPosition.End)]
        [InlineData(HorizontalPosition.Left)]
        [InlineData(HorizontalPosition.Right)]
        [InlineData(HorizontalPosition.Start)]
        [InlineData(HorizontalPosition.Unset)]
        public void RenderProperly_HorizontalDefaultPositionAttribute(HorizontalPosition horizontalPosition)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.HorizontalDefaultPosition, horizontalPosition)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              $"horizontal-default-position=\"{horizontalPosition.ToAttributeValue()}\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_HorizontalViewportLockAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.HorizontalViewportLock, true)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-viewport-lock=\"\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_HorizontalInsetAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.HorizontalInset, true)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-inset=\"\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_HorizontalThresholdAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.HorizontalThreshold, 10)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"10\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(AxisScalingMode.Anchor)]
        [InlineData(AxisScalingMode.Content)]
        [InlineData(AxisScalingMode.Fill)]
        public void RenderProperly_HorizontalScalingAttribute(AxisScalingMode axisScalingMode)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.HorizontalScaling, axisScalingMode)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              $"horizontal-scaling=\"{axisScalingMode.ToAttributeValue()}\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(AxisPositioningMode.Dynamic)]
        [InlineData(AxisPositioningMode.Locktodefault)]
        [InlineData(AxisPositioningMode.Uncontrolled)]
        public void RenderProperly_VerticalPositioningModeAttribute(AxisPositioningMode axisPositioningMode)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.VerticalPositioningMode, axisPositioningMode)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              $"vertical-positioning-mode=\"{axisPositioningMode.ToAttributeValue()}\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(VerticalPosition.Unset)]
        [InlineData(VerticalPosition.Bottom)]
        [InlineData(VerticalPosition.Top)]
        public void RenderProperly_VerticalDefaultPositionAttribute(VerticalPosition verticalPosition)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.VerticalDefaultPosition, verticalPosition)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              $"vertical-default-position=\"{verticalPosition.ToAttributeValue()}\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_VerticalViewportLockAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.VerticalViewportLock, true)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-viewport-lock=\"\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_VerticalInsetAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.VerticalInset, true)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-inset=\"\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_VerticalThresholdAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.VerticalThreshold, 100)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"100\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(AxisScalingMode.Content)]
        [InlineData(AxisScalingMode.Anchor)]
        [InlineData(AxisScalingMode.Fill)]
        public void RenderProperly_VerticalScalingAttribute(AxisScalingMode axisScalingMode)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.VerticalScaling, axisScalingMode)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              $"vertical-scaling=\"{axisScalingMode.ToAttributeValue()}\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_FixedPlacementAttribute()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.FixedPlacement, true)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\" " +
                              "fixed-placement=\"\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Theory]
        [InlineData(AutoUpdateMode.Auto)]
        [InlineData(AutoUpdateMode.Anchor)]
        public void RenderProperly_AutoUpdateModeAttribute(AutoUpdateMode autoUpdateMode)
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.AutoUpdateMode, autoUpdateMode)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              $"auto-update-mode=\"{autoUpdateMode.ToAttributeValue()}\" >" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_WithAdditionalCSSClass()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.Class, "additional-css-class")
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "class=\"additional-css-class\" " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_WithAdditionalStyle()
        {
            // Arrange && Act
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .Add(p => p.Style, "background-color: black;")
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              "style=\"background-color: black;\" " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_WithASingleAdditionalAttribute()
        {
            // Arrange && Act
            string additionalAttributeName = "additional";
            string additionalAttributeValue = "additional-value";
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .AddUnmatched(additionalAttributeName, additionalAttributeValue)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              $"{additionalAttributeName}=\"{additionalAttributeValue}\" " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }

        [Fact]
        public void RenderProperly_WithMultipleAdditionalAttribute()
        {
            // Arrange && Act
            string additionalAttribute1Name = "additional1";
            string additionalAttribute1Value = "additional1-value";
            string additionalAttribute2Name = "additional2";
            string additionalAttribute2Value = "additional2-value";
            string content = "content";
            IRenderedComponent<FluentAnchoredRegion> cut = TestContext.RenderComponent<FluentAnchoredRegion>(
                parameters => parameters
                    .AddUnmatched(additionalAttribute1Name, additionalAttribute1Value)
                    .AddUnmatched(additionalAttribute2Name, additionalAttribute2Value)
                    .AddChildContent(content));

            // Assert
            cut.MarkupMatches("<fluent-anchored-region " +
                              $"{additionalAttribute1Name}=\"{additionalAttribute1Value}\" " +
                              $"{additionalAttribute2Name}=\"{additionalAttribute2Value}\" " +
                              "horizontal-positioning-mode=\"uncontrolled\" " +
                              "horizontal-default-position=\"unset\" " +
                              "horizontal-threshold=\"0\" " +
                              "horizontal-scaling=\"content\" " +
                              "vertical-positioning-mode=\"uncontrolled\" " +
                              "vertical-default-position=\"unset\" " +
                              "vertical-threshold=\"0\" " +
                              "vertical-scaling=\"content\" " +
                              "auto-update-mode=\"anchor\">" +
                              $"{content}" +
                              "</fluent-anchored-region>");
        }
    }
}