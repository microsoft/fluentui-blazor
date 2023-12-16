// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using Benchmarks.Runners;

var summary = BenchmarkRunner.Run<FluentIconBenchmarks>(DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true));

Console.WriteLine(summary);