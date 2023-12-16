using BenchmarkDotNet.Attributes;

namespace Benchmarks.Runners;

[MemoryDiagnoser]
public class FluentIconBenchmarks {

	[GlobalSetup]
	public void Setup() {

	}

	[Benchmark(Baseline = true, Description = "Original implementation (100k)")]
	public void Instantiate100kIcon_Original() {
		for (var i = 0; i<100000;i++) {
			_ = new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.PresenceAvailable();
		}
	}

	[Benchmark(Baseline = false, Description = "Original with tuple implementation (100k)")]
	public void Instantiate100kIcon_WithTupleArray() {
		for (var i = 0; i < 100000; i++) {
			_ = new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size20.PresenceAvailableWithTupleArray();
		}
	}

	[Benchmark(Baseline = false, Description = "Resizable implementation (100k)")]
	public void Instantiate100kIcon_Sizable() {
		for (var i = 0; i < 100000; i++) {
			_ = new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.PresenceAvailable(20);
		}
	}
}
