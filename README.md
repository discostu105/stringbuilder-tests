Follow up on this stackoverflow question: https://stackoverflow.com/questions/53180372/fastest-way-to-concatenate-readonlyspanchar-in-c-sharp

Experiments to try to create a non-allocating StringBuilder that's as fast as possible.

__The code in this repository is totally experimental and not well tested at all. Use at your own risk!__

``` ini

BenchmarkDotNet=v0.11.2, OS=Windows 10.0.17134.345 (1803/April2018Update/Redstone4)
Intel Core i5-2500K CPU 3.30GHz (Sandy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=2.1.403
  [Host]     : .NET Core 2.1.5 (CoreCLR 4.6.26919.02, CoreFX 4.6.26919.02), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.5 (CoreCLR 4.6.26919.02, CoreFX 4.6.26919.02), 64bit RyuJIT


```
|                                      Method |      Mean |     Error |    StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|-------------------------------------------- |----------:|----------:|----------:|------------:|------------:|------------:|--------------------:|
|           ConcatSpansStringBuilderBenchmark | 142.06 ns | 2.8758 ns | 4.2153 ns |      0.0966 |           - |           - |               304 B |
|                  ConcatSpansCopyToBenchmark | 110.08 ns | 2.8908 ns | 8.3407 ns |      0.0813 |           - |           - |               256 B |
|       ConcatSpansFastStringBuilderBenchmark | 115.91 ns | 2.3242 ns | 2.5834 ns |      0.0813 |           - |           - |               256 B |
| ConcatSpansFastBufferStringBuilderBenchmark |  94.12 ns | 1.4318 ns | 1.3393 ns |      0.0407 |           - |           - |               128 B |
| ConcatSpansFastUnsafeStringBuilderBenchmark |  67.93 ns | 0.7748 ns | 0.6868 ns |      0.0407 |           - |           - |               128 B |
