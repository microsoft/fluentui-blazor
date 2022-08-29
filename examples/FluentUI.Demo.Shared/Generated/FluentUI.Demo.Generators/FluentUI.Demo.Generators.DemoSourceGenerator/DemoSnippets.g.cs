#pragma warning disable CS1591
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentUI.Demo.Generators;
public static class DemoSnippets
{
	public static string GetRazor(string name)
	{
		var metadata = new Dictionary<string,string>() {
		{ @"AccordionDefaultExample.razor", @"
<FluentAccordion ActiveId=""@activeId"" > @*OnAccordionItemChange=""HandleOnAccordionItemChange""*@
    <FluentAccordionItem Expanded=""true"" Heading=""Panel one"">
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
        Panel one content, using the 'start' slot for extra header content (in this case an icon)
    </FluentAccordionItem>
    <FluentAccordionItem Heading=""Panel two"">
        <div slot=""end"">
            <button>1</button>
        </div>
        Panel two content, using the 'end' slot for extra header content (in this case an HTML button)
    </FluentAccordionItem>
    <FluentAccordionItem Expanded=""true"" Heading=""Panel three"">
        Panel three content
    </FluentAccordionItem>
</FluentAccordion>

<p>Last changed accordion item: @changed?.Heading</p>


@code {
    string activeId = ""accordion-1"";

    FluentAccordionItem? changed;

    private void HandleOnAccordionItemChange(FluentAccordionItem item)
    {
        changed = item;
    }
}" },
		{ @"AccordionSingleExpandExample.razor", @"<FluentAccordion ExpandMode=""AccordionExpandMode.Single"">
    <FluentAccordionItem Heading=""Panel one"">
        Panel one contet
    </FluentAccordionItem>
    <FluentAccordionItem Heading=""Panel two"">
        Panel two content
    </FluentAccordionItem>
    <FluentAccordionItem Heading=""Panel three"">
        Panel three content
    </FluentAccordionItem>
</FluentAccordion>" },
		{ @"AnchoredRegionExamples.razor", @"
<div style=""height:100%;overflow:hidden;display:flex;flex-direction:column"">
    <div style=""overflow:auto;height:100%"">
        <h4>Dynamic - default</h4>
        <div id=""viewport-default"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px"">
                <button id=""anchor-default"" style=""margin-left:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion Anchor=""anchor-default"" Viewport=""viewport-default"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" HorizontalScaling=""AxisScalingMode.Content"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top left"" style=""position: absolute; transform: translate(400px, -121px); width: unset; height: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Lock to default</h4>
        <div id=""viewport-locked"" style=""height:400px;width:400px;background:#d3d3d3;overflow:scroll;position:relative"">
            <div style=""height:1000px;width:1000px"">
                <FluentAnchoredRegion Anchor=""anchor-locked"" Viewport=""viewport-locked"" VerticalPositioningMode=""AxisPositioningMode.Locktodefault"" VerticalDefaultPosition=""VerticalPosition.Bottom"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Right"" HorizontalScaling=""AxisScalingMode.Content"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded bottom right"" style=""position: absolute; transform: translate(557px, 521px); width: unset; height: unset;"">
                    <div style=""height:150px;width:150px;background:green"" />
                </FluentAnchoredRegion>
                <div style=""position:relative;height:0;width:0"">
                    <FluentAnchoredRegion Anchor=""anchor-locked"" Viewport=""viewport-locked"" VerticalPositioningMode=""AxisPositioningMode.Locktodefault"" VerticalDefaultPosition=""VerticalPosition.Bottom"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Right"" HorizontalScaling=""AxisScalingMode.Content"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded bottom right"" style=""position: absolute; transform: translate(557px, 521px); width: unset; height: unset;"">
                        <div style=""height:100px;width:100px;background:#ff0"" />
                    </FluentAnchoredRegion>
                </div>
                <FluentAnchoredRegion Anchor=""anchor-locked"" Viewport=""viewport-locked"" VerticalPositioningMode=""AxisPositioningMode.Locktodefault"" VerticalDefaultPosition=""VerticalPosition.Bottom"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Right"" HorizontalScaling=""AxisScalingMode.Content"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded bottom right"" style=""position: absolute; transform: translate(557px, 521px); width: unset; height: unset;"">
                    <div style=""height:50px;width:50px;background:#00f"" />
                </FluentAnchoredRegion>
                <div />
                <button id=""anchor-locked"" style=""margin-left:500px;margin-top:500px"">anchor</button>
            </div>
        </div>
        <h4>Scaling via update</h4>
        <div id=""viewport-scaling-update"" style=""height:400px;width:400px;background:#d3d3d3;overflow:scroll;position:relative"">
            <div style=""height:1000px;width:1000px;overflow:hidden"">
                <button id=""anchor-scaling-update"" style=""margin-left:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion id=""region-scaling-update"" Anchor=""anchor-scaling-update"" Viewport=""viewport-scaling-update"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalScaling=""AxisScalingMode.Fill"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalScaling=""AxisScalingMode.Fill"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" VerticalDefaultPosition=""VerticalPosition.Unset"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top left"" style=""position: absolute; transform: translate(0px, -521px); width: 500px; height: 500px;"">
                    <div style=""height:100%;width:100%;background:#ff0"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Scaling via offset</h4>
        <div id=""viewport-scaling-offset"" style=""height:400px;width:400px;background:#d3d3d3;overflow:scroll;position:relative"">
            <div style=""height:1000px;width:1000px;overflow:hidden"">
                <button id=""anchor-scaling-offset"" style=""margin-left:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion id=""region-scaling-offset"" Anchor=""anchor-scaling-offset"" Viewport=""viewport-scaling-offset"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalScaling=""AxisScalingMode.Fill"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalScaling=""AxisScalingMode.Fill"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" VerticalDefaultPosition=""VerticalPosition.Unset"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top left"" style=""position: absolute; transform: translate(0px, -521px); width: 500px; height: 500px;"">
                    <div style=""height:100%;width:100%;background:#ff0"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Inset</h4>
        <div id=""viewport-inset"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px;overflow:hidden"">
                <button id=""anchor-inset"" style=""margin-left:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion Anchor=""anchor-inset"" Viewport=""viewport-inset"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalInset=""false"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalInset=""false"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" HorizontalScaling=""AxisScalingMode.Content"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded inset-top inset-left"" style=""position: absolute; transform: translate(457px, -100px); width: unset; height: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0;opacity:.5"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Thresholds</h4>
        <div id=""viewport-thresholds"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px;overflow:hidden"">
                <button id=""anchor-thresholds"" style=""margin-left:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion Anchor=""anchor-thresholds"" Viewport=""viewport-thresholds"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalDefaultPosition=""VerticalPosition.Top"" vertical-threshold=""0"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalDefaultPosition=""HorizontalPosition.Left"" HorizontalThreshold=""200"" HorizontalScaling=""AxisScalingMode.Content"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top left"" style=""position: absolute; transform: translate(400px, -121px); width: unset; height: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Toggle anchor</h4>
        <div id=""viewport-toggle-anchor"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px;overflow:hidden"">
                <button id=""toggle-anchor-anchor1"" style=""margin-left:400px;margin-top:500px"">Set anchor 1</button>
                <button id=""toggle-anchor-anchor2"">Set anchor 2</button>
                <FluentAnchoredRegion id=""toggle-anchor-region"" Anchor=""toggle-anchor-anchor1"" Viewport=""viewport-toggle-anchor"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" HorizontalScaling=""AxisScalingMode.Content"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top left"" style=""position: absolute; transform: translate(300px, -121px); width: unset; height: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Toggle positions and size</h4>
        <div id=""viewport-toggle-positions"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px;overflow:hidden"">
                <button id=""anchor-toggle-positions"" style=""margin-left:400px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion id=""toggle-positions-region"" Anchor=""anchor-toggle-positions"" Viewport=""viewport-toggle-positions"" VerticalPositioningMode=""AxisPositioningMode.Locktodefault"" VerticalDefaultPosition=""VerticalPosition.Bottom"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Right"" HorizontalScaling=""AxisScalingMode.Content"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded bottom right"" style=""position: absolute; transform: translate(457px, 0px); width: unset; height: unset;"">
                    <div id=""toggle-positions-small"" style=""height:50px;width:50px;background:#ff0"" hidden />
                    <div id=""toggle-positions-large"" style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
                <div style=""margin-top:20px;display:flex;flex-direction:row"">
                    <button id=""toggle-positions-horizontal"" style=""margin-left:250px;margin-top:120px""> toggle horizontal </button>
                    <button id=""toggle-positions-vertical"" style=""margin-top:120px"">toggle vertical</button>
                    <button id=""toggle-positions-small"" style=""margin-top:120px"">small</button>
                    <button id=""toggle-positions-large"" style=""margin-top:120px"">large</button>
                </div>
            </div>
        </div>
        <h4>RTL-dynamic</h4>
        <div id=""viewport-rtl-dynamic"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"" dir=""rtl"">
            <div style=""height:1000px;width:1000px"">
                <FluentAnchoredRegion Anchor=""anchor-rtl-dynamic"" Viewport=""viewport-rtl-dynamic"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalDefaultPosition=""VerticalPosition.Top"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" HorizontalScaling=""AxisScalingMode.Content"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top right"" style=""position: absolute; transform: translate(-399.766px, 400px); width: unset; height: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
                <button id=""anchor-rtl-dynamic"" style=""margin-right:500px;margin-top:500px"">anchor</button>
            </div>
        </div>
        <h4>Size to anchor</h4>
        <div id=""viewport-anchor-sized"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px"">
                <FluentAnchoredRegion Anchor=""anchor-anchor-sized"" Viewport=""viewport-anchor-sized"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalScaling=""AxisScalingMode.Anchor"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalScaling=""AxisScalingMode.Anchor"" HorizontalInset=""false"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" VerticalDefaultPosition=""VerticalPosition.Unset"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top inset-left"" style=""position: absolute; transform: translate(500px, 479px); width: 57px; height: 21px;"">
                    <div style=""height:100%;width:100%;background:#ff0"" />
                </FluentAnchoredRegion>
                <button id=""anchor-anchor-sized"" style=""margin-left:500px;margin-top:500px"">anchor</button>
            </div>
        </div>
        <h4>RTL-fill</h4>
        <div id=""viewport-rtl-fill"" dir=""rtl"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px"">
                <button id=""anchor-rtl-fill"" style=""margin-right:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion Anchor=""anchor-rtl-fill"" Viewport=""viewport-rtl-fill"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalScaling=""AxisScalingMode.Fill"" HorizontalPositioningMode=""AxisPositioningMode.Dynamic"" HorizontalScaling=""AxisScalingMode.Fill"" HorizontalDefaultPosition=""HorizontalPosition.Unset"" VerticalDefaultPosition=""VerticalPosition.Unset"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded top right"" style=""position: absolute; transform: translate(0px, -521px); width: 499.766px; height: 500px;"">
                    <div style=""height:100%;width:100%;background:#ff0"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>Start &amp; End</h4>
        <div id=""viewport-se"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px"">
                <button id=""anchor-se"" style=""margin-left:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion Anchor=""anchor-se"" Viewport=""viewport-se"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Start"" HorizontalScaling=""AxisScalingMode.Content"" VerticalPositioningMode=""AxisPositioningMode.Uncontrolled"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded left"" style=""position: absolute; transform: translate(400px, 0px); width: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
                <FluentAnchoredRegion Anchor=""anchor-se"" Viewport=""viewport-se"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.End"" HorizontalScaling=""AxisScalingMode.Content"" VerticalPositioningMode=""AxisPositioningMode.Uncontrolled"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded right"" style=""position: absolute; transform: translate(557px, 0px); width: unset;"">
                    <div style=""height:100px;width:100px;background:green"" />
                </FluentAnchoredRegion>
            </div>
        </div>
        <h4>RTL-Start &amp; End</h4>
        <div id=""viewport-rtl-se"" dir=""rtl"" style=""position:relative;height:400px;width:400px;background:#d3d3d3;overflow:scroll"">
            <div style=""height:1000px;width:1000px"">
                <button id=""anchor-rtl-se"" style=""margin-right:500px;margin-top:500px"">anchor</button>
                <FluentAnchoredRegion Anchor=""anchor-rtl-se"" Viewport=""viewport-rtl-se"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Start"" HorizontalScaling=""AxisScalingMode.Content"" VerticalPositioningMode=""AxisPositioningMode.Uncontrolled"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded right"" style=""position: absolute; transform: translate(-399.766px, 0px); width: unset;"">
                    <div style=""height:100px;width:100px;background:#ff0"" />
                </FluentAnchoredRegion>
                <FluentAnchoredRegion Anchor=""anchor-rtl-se"" Viewport=""viewport-rtl-se"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.End"" HorizontalScaling=""AxisScalingMode.Content"" VerticalPositioningMode=""AxisPositioningMode.Uncontrolled"" VerticalDefaultPosition=""VerticalPosition.Unset"" VerticalScaling=""AxisScalingMode.Content"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded left"" style=""position: absolute; transform: translate(-556.766px, 0px); width: unset;"">
                    <div style=""height:100px;width:100px;background:green"" />
                </FluentAnchoredRegion>
            </div>
        </div>
    </div>
    <FluentAnchoredRegion Anchor=""anchor-fixed"" VerticalPositioningMode=""AxisPositioningMode.Dynamic"" VerticalScaling=""AxisScalingMode.Anchor"" VerticalInset=""false"" HorizontalPositioningMode=""AxisPositioningMode.Locktodefault"" HorizontalDefaultPosition=""HorizontalPosition.Left"" fixed-placement="""" style=""z-index: 11; position: fixed; transform: translate(330.047px, 20px); width: unset; height: 20px;"" Viewport="""" HorizontalScaling=""AxisScalingMode.Content"" VerticalDefaultPosition=""VerticalPosition.Unset"" AutoUpdateMode=""AutoUpdateMode.Anchor"" class=""loaded inset-bottom left"">
        <div style=""height:100%;width:100%;background:#00f;z-index:11"">outside &amp; fixed</div>
    </FluentAnchoredRegion>
</div>" },
		{ @"AnchorDefaultExamples.razor", @"<h4>Default</h4>
<FluentAnchor Href=""#"">Anchor</FluentAnchor>

<h5>With target</h5>
<FluentAnchor Href=""https://microsoft.com"" Target=""_blank"">Anchor</FluentAnchor>

<h4>Neutral</h4>
<FluentAnchor Href=""#"" Appearance=""Appearance.Neutral"">Button</FluentAnchor>

<h4>Accent</h4>
<FluentAnchor Href=""#"" Appearance=""Appearance.Accent"">Anchor</FluentAnchor>

<h4>Hypertext</h4>
<FluentAnchor Href=""#"" Appearance=""Appearance.Hypertext"">Anchor</FluentAnchor>
<br />
<FluentAnchor Appearance=""Appearance.Hypertext"">Anchor (no Href)</FluentAnchor>
<p>
    Lorem ipsum dolor sit amet consectetur adipisicing elit. Nesciunt ut aliquam quas quod ipsam cupiditate, voluptate,
    corrupti
    <FluentAnchor Appearance=""Appearance.Hypertext"" Href=""#"">doloremque totam</FluentAnchor>
    dicta perspiciatis commodi consequatur reprehenderit laborum aliquid minima. Neque, recusandae. Adipisci.
</p>

<h4>Lightweight</h4>
<FluentAnchor Href=""#"" Appearance=""Appearance.Lightweight"">Anchor</FluentAnchor>

<h4>Outline</h4>
<FluentAnchor Href=""#"" Appearance=""Appearance.Outline"">Anchor</FluentAnchor>

<h4>Stealth</h4>
<FluentAnchor Href=""#"" Appearance=""Appearance.Stealth"">Anchor</FluentAnchor>

<h4>With start</h4>
<FluentAnchor Href=""#"">
    Anchor
    <FluentIcon Slot=""start"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
</FluentAnchor>

<h4>With end</h4>
<FluentAnchor Href=""#"">
    <FluentIcon Slot=""end"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    Anchor

</FluentAnchor>

<h4>Icon in default slot</h4>
<FluentAnchor Href=""#"">
    <FluentIcon Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false />
</FluentAnchor>

<h4>With aria-label</h4>
<FluentAnchor Href=""#"" aria-label=""Anchor with aria-label""></FluentAnchor>" },
		{ @"BadgeAppearanceExamples.razor", @"<FluentBadge Appearance=""Appearance.Lightweight"">
    Lightweight
</FluentBadge>

<FluentBadge Appearance=""Appearance.Accent"">
    Accent
</FluentBadge>

<FluentBadge Appearance=""Appearance.Neutral"">
    Neutral
</FluentBadge>" },
		{ @"BadgeCircularExample.razor", @"<FluentBadge Circular=true Appearance=""Appearance.Accent"">
    Circular
</FluentBadge>" },
		{ @"BadgeDefaultExample.razor", @"<h4>Default</h4>
<FluentBadge>
    Badge
</FluentBadge>" },
		{ @"BadgeStyledExamples.razor", @"<p>
    The Fill and Color values can be defined as CSS variables with 'highlight' and 'lowlight' variants:
    <code>
        <pre>
        fluent-badge {
            --badge-fill-highlight: #ffd800;
            --badge-fill-lowlight: #333;
            --badge-color-highlight: #000;
            --badge-color-lowlight: #fff;
        }
        </pre>
    </code>

    <FluentBadge Fill=""highlight"" Color=""highlight"">
        Badge
    </FluentBadge>
    <FluentBadge Fill=""lowlight"" Color=""lowlight"">
        Badge
    </FluentBadge>
</p>" },
		{ @"BreadcrumbDefaultExample.razor", @"<FluentBreadcrumb>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 1
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 2
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem>Breadcrumb item 3</FluentBreadcrumbItem>
</FluentBreadcrumb>
" },
		{ @"BreadcrumbItemDefaultExample.razor", @"<FluentBreadcrumbItem Href=""#"">
    Breadcrumb item
</FluentBreadcrumbItem>
" },
		{ @"BreadcrumbItemWithAllExample.razor", @"<FluentBreadcrumbItem Href=""#"">
    Breadcrumb item
    <FluentIcon Slot=""separator"" Name=""@FluentIcons.ChevronDoubleRight"" Size=""@IconSize.Size20"" Filled=false UseAccentColor=false />
    <FluentIcon Slot=""end"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    <FluentIcon Slot=""start"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
</FluentBreadcrumbItem>
" },
		{ @"BreadcrumbItemWithEndExample.razor", @"<FluentBreadcrumbItem Href=""#"">
    Breadcrumb item
    <FluentIcon Slot=""end"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
</FluentBreadcrumbItem>" },
		{ @"BreadcrumbItemWithSeparatorExample.razor", @"<FluentBreadcrumbItem Href=""#"">
    Breadcrumb item
    <FluentIcon Slot=""separator"" Name=""@FluentIcons.ChevronDoubleRight"" Size=""@IconSize.Size20"" Filled=false UseAccentColor=false />
</FluentBreadcrumbItem>" },
		{ @"BreadcrumbItemWithStartExample.razor", @"<FluentBreadcrumbItem Href=""#"">
    Breadcrumb item
    <FluentIcon Slot=""start"" Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
</FluentBreadcrumbItem>" },
		{ @"BreadcrumbWithAllExample.razor", @"<FluentBreadcrumb>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 1
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Home"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
        <FluentIcon Slot=""end"" Name=""@FluentIcons.Home"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
        <FluentIcon Slot=""separator"" Name=""@FluentIcons.ChevronDoubleRight"" Size=""@IconSize.Size20"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 2
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Clipboard"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
        <FluentIcon Slot=""end"" Name=""@FluentIcons.Clipboard"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
        <FluentIcon Slot=""separator"" Name=""@FluentIcons.ChevronDoubleRight"" Size=""@IconSize.Size20"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem>Breadcrumb item 3
        <FluentIcon Slot=""end"" Name=""@FluentIcons.Money"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Money"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
</FluentBreadcrumb>" },
		{ @"BreadcrumbWithEndExample.razor", @"<FluentBreadcrumb>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 1
        <FluentIcon Slot=""end"" Name=""@FluentIcons.Home"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 2
        <FluentIcon Slot=""end"" Name=""@FluentIcons.Clipboard"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem>
        Breadcrumb item 3
        <FluentIcon Slot=""end"" Name=""@FluentIcons.Money"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
</FluentBreadcrumb>" },
		{ @"BreadcrumbWithSeparatorExample.razor", @"<FluentBreadcrumb>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 1
        <FluentIcon Slot=""separator"" Name=""@FluentIcons.ChevronDoubleRight"" Size=""@IconSize.Size20"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 2
        <FluentIcon Slot=""separator"" Name=""@FluentIcons.ChevronDoubleRight"" Size=""@IconSize.Size20"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem>Breadcrumb item 3</FluentBreadcrumbItem>
</FluentBreadcrumb>" },
		{ @"BreadcrumbWithStartExample.razor", @"<FluentBreadcrumb>
    <FluentBreadcrumbItem Href=""#"">
         Breadcrumb item 1
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Home"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem Href=""#"">
        Breadcrumb item 2
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Clipboard"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
    <FluentBreadcrumbItem>
        Breadcrumb item 3
        <FluentIcon Slot=""start"" Name=""@FluentIcons.Money"" Size=""@IconSize.Size16"" Filled=false UseAccentColor=false />
    </FluentBreadcrumbItem>
</FluentBreadcrumb>
" },
		{ @"ButtonAppearanceExamples.razor", @"<p>Neutral and Neutral with Disabled</p>
<FluentButton Appearance=""Appearance.Neutral"">Button</FluentButton>
<FluentButton Appearance=""Appearance.Neutral"" Disabled=""true"">Button</FluentButton>

<p>Accent and Accent with Disabled</p>
<FluentButton Appearance=""Appearance.Accent"">Button</FluentButton>
<FluentButton Appearance=""Appearance.Accent"" Disabled=""true"">Button</FluentButton>

<p>Lightweight and Lightweight with Disabled</p>
<FluentButton Appearance=""Appearance.Lightweight"">Button</FluentButton>
<FluentButton Appearance=""Appearance.Lightweight"" Disabled=""true"">Button</FluentButton>

<p>Outline and Ouline with Disabled</p>
<FluentButton Appearance=""Appearance.Outline"">Button</FluentButton>
<FluentButton Appearance=""Appearance.Outline"" Disabled=""true"">Button</FluentButton>

<p>Stealth and Stealth with Disabled</p>
<FluentButton Appearance=""Appearance.Stealth"">Button</FluentButton>
<FluentButton Appearance=""Appearance.Stealth"" Disabled=""true"">Button</FluentButton>
" },
		{ @"ButtonAriaExample.razor", @"<FluentButton aria-label=""Button with aria-label"">Button</FluentButton>" },
		{ @"ButtonDefaultExamples.razor", @"<p>Default</p>
<FluentButton>Button</FluentButton>

<p>Disabled</p>
<FluentButton Disabled=""true"">Button</FluentButton>

<p>Autofocus</p>
<FluentButton Autofocus=""true"">Button</FluentButton>" },
		{ @"ButtonIconExamples.razor", @"<p>With icon at start</p>
<FluentButton>
    Button
    <FluentIcon Name=""@FluentIcons.Globe"" Slot=""start"" Size=""@IconSize.Size16"" Filled=false />
</FluentButton>

<p>With icon at end</p>
<FluentButton>
    <FluentIcon Name=""@FluentIcons.Globe"" Slot=""end"" Size=""@IconSize.Size16"" Filled=false />
    Button
</FluentButton>

<h4>With icon in default slot</h4>
<FluentButton>
    <FluentIcon Name=""@FluentIcons.Globe"" Size=""@IconSize.Size16"" Filled=false />
</FluentButton>" },
		{ @"CalendarDefaultExamples.razor", @"<h5>Default (clickable) calendar</h5>
<FluentCalendar OnDateClicked=""HandleDateClicked"" />
<p>You clicked on @clickedDate</p>

<h5>Readonly</h5>
<FluentCalendar Readonly=true />

<h5>Show a minumum of 6 weeks</h5>
<FluentCalendar MinWeeks=""6"" />

<h5>2-digit day format</h5>
<FluentCalendar DayFormat=DayFormat.TwoDigit />

@code {
    private DateOnly? clickedDate = null;

    private void HandleDateClicked(DateOnly date)
    {
        clickedDate = date;
    }
}
" },
		{ @"CalendarInteractiveExamples.razor", @"<h5>Selected dates (with @@bind)</h5>
<FluentCalendar DisabledDates=@disabledDates @bind-SelectedDates =@selectedDates></FluentCalendar>

<p>Selected dates</p>
<ul>
    @foreach (DateOnly date in selectedDates)
    {
        <li>@date.ToString(""yyyy-MM-dd"")</li>
    }
</ul>

<h5>Selected dates (with @@bind) and diasabled dates not selectable</h5>
<FluentCalendar DisabledDates=@disabledDates @bind-SelectedDates=@selectedDates DisabledSelectable=false></FluentCalendar>

<h5>Selected dates (with @@bind) and dates outside of shown month not selectable</h5>
<FluentCalendar DisabledDates=@disabledDates @bind-SelectedDates=@selectedDates OutOfMonthSelectable=false></FluentCalendar>



@code {
    static int currentYear = DateTime.Now.Year;
    static int currentMonth = DateTime.Now.Month;

    static int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth) + 1;
    static Random random = new();

    List<DateOnly> disabledDates = new();
    List<DateOnly> selectedDates = new();

    protected override void OnInitialized()
    {
        for (int i = 0; i < 3; i++)
        {
            disabledDates.Add(new DateOnly(currentYear, currentMonth, random.Next(1, daysInMonth)));
            selectedDates.Add(new DateOnly(currentYear, currentMonth, random.Next(1, daysInMonth)));
        };
    }
}
" },
		{ @"CardDefaultExamples.razor", @"<div>
    <FluentCard class=""state-override"">Custom size using CSS</FluentCard>

    <BaseLayerLuminance Value=""(float?)0.15"">
        <FluentCard BackReference=""@context"">
            <div class=""contents"">
                <p>Dark</p>
                <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
                <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
                <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
                <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
            </div>
        </FluentCard>
    </BaseLayerLuminance>

    <FluentDesignSystemProvider FillColor=""#D6D6D6"">
        <FluentCard neutral-palette-source=""#CABA8C"">
            <div class=""contents"">
                <p>Tinted neutral-palette-source, dark container</p>
                <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
                <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
                <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
                <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
            </div>
        </FluentCard>
    </FluentDesignSystemProvider>

    <BaseLayerLuminance Value=""(float?)0.15"">
        <FluentCard neutral-palette-source=""#CABA8C"" BackReference=""@context"">
            <div class=""contents"">
                <p>Tinted neutral-palette-source, dark</p>
                <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
                <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
                <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
                <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
            </div>
        </FluentCard>
    </BaseLayerLuminance>

    <FluentDesignSystemProvider BaseLayerLuminance=""(float?)0.15"">
        <FluentCard neutral-palette-source=""#CABA8C"">
            <div class=""contents"">
                <p>Tinted neutral-palette-source, dark</p>
                <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
                <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
                <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
                <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
            </div>
            <FluentCard neutral-palette-source=""#718E71"" style=""margin: 0; --card-height: 200px; --card-width: 460px;"" >
                <div class=""contents"">
                    <p>Tinted neutral-palette-source, nested, dark</p>
                    <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
                    <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
                    <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
                    <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
                </div>
            </FluentCard>
        </FluentCard>
    </FluentDesignSystemProvider>

    <FluentCard card-fill-color=""#449544"">
        <div class=""contents"">
            <p>Custom card-fill-color</p>
            <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
            <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
            <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
            <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
            <p>
                Note the stealth buttons have a slight fill, which is because the card-fill-color is explicit, but the stealth
                recipe gets its value from the neutral palette which has been created based on the card-fill-color, but does not
                contain that exact color.
            </p>
        </div>
    </FluentCard>

    <FluentDesignSystemProvider FillColor=""#D6D6D6"" AccentBaseColor=""#718E71"" NeutralBaseColor=""#A90000"">
        <FluentCard>
            <div class=""contents"">
                <p>Accent and neutral color by DSP</p>
                <FluentButton Appearance=""Appearance.Accent"">Accent</FluentButton>
                <FluentButton Appearance=""Appearance.Neutral"">Neutral</FluentButton>
                <FluentButton Appearance=""Appearance.Stealth"">Stealth</FluentButton>
                <FluentButton Appearance=""Appearance.Outline"">Outline</FluentButton>
                <FluentButton Appearance=""Appearance.Lightweight"">Lightweight</FluentButton>
            </div>
        </FluentCard>
    </FluentDesignSystemProvider>
</div>" },
		{ @"CheckboxAriaExample.razor", @"<FluentCheckbox>
    <span aria-label=""Audio label"">Visible label</span>
</FluentCheckbox>" },
		{ @"CheckboxDefaultExamples.razor", @"<h5>Standard</h5>
<p>Without a label: <FluentCheckbox></FluentCheckbox></p>
<p>With a label: <FluentCheckbox>label</FluentCheckbox></p>

<h5>Checked</h5>
<FluentCheckbox Checked=""true""></FluentCheckbox>

<!-- Required -->
<h5>Required</h5>
<FluentCheckbox Required=""true""></FluentCheckbox>

<!-- Disabled -->
<h5>Disabled</h5>
<FluentCheckbox Disabled=""true""></FluentCheckbox>
<FluentCheckbox Disabled=""true"">label</FluentCheckbox>
<FluentCheckbox Disabled=""true"" Checked=""true"">Checked=""true""</FluentCheckbox>

<h5>Inline</h5>
<FluentCheckbox Checked=""true"">Apples</FluentCheckbox>
<FluentCheckbox Checked=""true"">Bananas</FluentCheckbox>
<FluentCheckbox>Honeydew</FluentCheckbox>
<FluentCheckbox Checked=""true"">Oranges</FluentCheckbox>" },
		{ @"CheckboxVerticalExample.razor", @"
<fieldset style=""display: flex; flex-direction: column; align-items: start;"">
    <legend>Fruit</legend>
    <FluentCheckbox Checked=""true"">Apples</FluentCheckbox>
    <FluentCheckbox Checked=""true"">Bananas</FluentCheckbox>
    <FluentCheckbox>Honeydew</FluentCheckbox>
    <FluentCheckbox Checked=""true"">Oranges</FluentCheckbox>
</fieldset>" },
		};
		var foundPair = metadata.FirstOrDefault(x => x.Key.EndsWith(name + ".razor" ));
		return foundPair.Value;
		}
}
