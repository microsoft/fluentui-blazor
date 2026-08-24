// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;
using Microsoft.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.Components.Overflow;

[Collection(StartServerCollection.Name)]
public class FluentOverflowTests : FluentPlaywrightBaseTest
{
    public FluentOverflowTests(ITestOutputHelper output, StartServerFixture server)
        : base(output, server)
    {
    }

    [Fact(Skip = "Playwright is optional for the moment")]
    public async Task FluentOverflow_AutomaticRefreshes_AreCoalesced()
    {
        var page = await WaitOpenPageAsync("/overflow/scheduling", openDevTools: false);

        var result = await page.EvaluateAsync<OverflowSchedulingResult>(Script);

        Assert.Equal(result.ElementCount, result.InitialMeasurements);
        Assert.True(result.HiddenBeforeMeasurement);
        Assert.True(result.VisibleAfterMeasurement);
        Assert.Equal(result.ManagedItemCount, result.ManagedItemsAfterResize);
        Assert.Equal(result.NormalOverflowCount, result.RestoredOverflowCount);
        Assert.True(result.NarrowOverflowCount > result.NormalOverflowCount);
        Assert.Equal(Math.Min(result.NormalOverflowCount, 3), result.NormalPayloadCount);
        Assert.Equal(Math.Min(result.NarrowOverflowCount, 3), result.NarrowPayloadCount);
        Assert.Equal($"+{result.NormalOverflowCount}", result.NormalIndicator);
        Assert.Equal($"+{result.NarrowOverflowCount}", result.NarrowIndicator);
        Assert.Equal($"+{result.RestoredOverflowCount}", result.RestoredIndicator);
        Assert.Equal(result.MeasurementsBeforeFlush + 1, result.MeasurementsAfterFlush);
        Assert.Equal(result.MeasurementsAfterFlush, result.MeasurementsAfterSettling);
        Assert.Equal(result.MeasurementsBeforeDisconnect, result.MeasurementsAfterDisconnect);
    }

    private const string Script = """
        async () => {
            const elementCount = 220;
            const managedItemsPerElement = 6;
            const host = document.querySelector("#overflow-test-host");
            const overflowPrototype = customElements.get("fluent-overflow").prototype;
            const originalRefreshNow = overflowPrototype.refreshNow;
            let measurementExecutions = 0;

            overflowPrototype.refreshNow = function (...args) {
                measurementExecutions++;
                return originalRefreshNow.apply(this, args);
            };

            const nextFrame = () => new Promise(resolve => requestAnimationFrame(resolve));
            const delay = milliseconds => new Promise(resolve => setTimeout(resolve, milliseconds));
            const waitForStableFrames = async elements => {
                let previousSignature = "";
                let stableFrameCount = 0;
                while (stableFrameCount < 5) {
                    await nextFrame();
                    const signature = elements
                        .map(element => `${element.querySelectorAll(".managed-item[overflow]").length}:${element.offsetWidth}`)
                        .join("|");
                    stableFrameCount = signature === previousSignature ? stableFrameCount + 1 : 0;
                    previousSignature = signature;
                }
            };

            const createOverflow = index => {
                const overflow = document.createElement("fluent-overflow");
                overflow.className = "test-overflow";
                overflow.id = `test-overflow-${index}`;
                overflow.setAttribute("visible-on-load", "false");
                overflow.setAttribute("max-rendered-items", "3");

                for (let itemIndex = 0; itemIndex < managedItemsPerElement; itemIndex++) {
                    const item = document.createElement("span");
                    item.className = "managed-item";
                    item.id = `test-item-${index}-${itemIndex}`;
                    item.textContent = `Item ${itemIndex + 1}`;
                    overflow.append(item);
                }

                const fixedItem = document.createElement("span");
                fixedItem.className = "fixed-item";
                fixedItem.setAttribute("behavior", "fixed");
                fixedItem.textContent = "+0";
                overflow.append(fixedItem);
                overflow.addEventListener("overflowchange", event => {
                    fixedItem.textContent = `+${event.detail.overflowCount}`;
                });
                return overflow;
            };

            try {
                const fragment = document.createDocumentFragment();
                for (let index = 0; index < elementCount; index++) {
                    fragment.append(createOverflow(index));
                }
                host.append(fragment);

                const elements = [...host.querySelectorAll("fluent-overflow")];
                elements.forEach((element, index) => {
                    element.setAttribute("threshold", String(20 + index % 3));
                    element.lastElementChild.classList.toggle("mutated", index % 2 === 0);
                });

                const hiddenBeforeMeasurement = elements.every(element => element.style.visibility === "hidden");
                await waitForStableFrames(elements);
                await delay(50);

                const initialMeasurements = measurementExecutions;
                const visibleAfterMeasurement = elements.every(element => element.style.visibility === "");
                const first = elements[0];
                const indicator = first.querySelector(".fixed-item");
                const normalState = first.getOverflowState();
                const normalIndicator = indicator.textContent;
                const managedItemCount = host.querySelectorAll(".managed-item").length;

                first.classList.add("narrow");
                await waitForStableFrames([first]);
                await delay(50);
                const narrowState = first.getOverflowState();
                const narrowIndicator = indicator.textContent;

                first.classList.remove("narrow");
                await waitForStableFrames([first]);
                await delay(50);
                const restoredState = first.getOverflowState();
                const restoredIndicator = indicator.textContent;

                first.setAttribute("threshold", "26");
                const measurementsBeforeFlush = measurementExecutions;
                first.getOverflowState();
                const measurementsAfterFlush = measurementExecutions;
                await waitForStableFrames([first]);
                const measurementsAfterSettling = measurementExecutions;

                const detached = createOverflow("detached");
                const measurementsBeforeDisconnect = measurementExecutions;
                host.append(detached);
                detached.remove();
                await waitForStableFrames([]);
                const measurementsAfterDisconnect = measurementExecutions;

                return {
                    elementCount,
                    managedItemCount,
                    managedItemsAfterResize: host.querySelectorAll(".managed-item").length,
                    initialMeasurements,
                    hiddenBeforeMeasurement,
                    visibleAfterMeasurement,
                    normalOverflowCount: normalState.overflowCount,
                    narrowOverflowCount: narrowState.overflowCount,
                    restoredOverflowCount: restoredState.overflowCount,
                    normalPayloadCount: normalState.overflowItems.length,
                    narrowPayloadCount: narrowState.overflowItems.length,
                    normalIndicator,
                    narrowIndicator,
                    restoredIndicator,
                    measurementsBeforeFlush,
                    measurementsAfterFlush,
                    measurementsAfterSettling,
                    measurementsBeforeDisconnect,
                    measurementsAfterDisconnect
                };
            } finally {
                overflowPrototype.refreshNow = originalRefreshNow;
            }
        }
        """;

    private sealed class OverflowSchedulingResult
    {
        public int ElementCount { get; set; }
        public int ManagedItemCount { get; set; }
        public int ManagedItemsAfterResize { get; set; }
        public int InitialMeasurements { get; set; }
        public bool HiddenBeforeMeasurement { get; set; }
        public bool VisibleAfterMeasurement { get; set; }
        public int NormalOverflowCount { get; set; }
        public int NarrowOverflowCount { get; set; }
        public int RestoredOverflowCount { get; set; }
        public int NormalPayloadCount { get; set; }
        public int NarrowPayloadCount { get; set; }
        public string NormalIndicator { get; set; } = string.Empty;
        public string NarrowIndicator { get; set; } = string.Empty;
        public string RestoredIndicator { get; set; } = string.Empty;
        public int MeasurementsBeforeFlush { get; set; }
        public int MeasurementsAfterFlush { get; set; }
        public int MeasurementsAfterSettling { get; set; }
        public int MeasurementsBeforeDisconnect { get; set; }
        public int MeasurementsAfterDisconnect { get; set; }
    }
}