"# stringbuilder-tests" 

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
|                                      Method |      Mean |    Error |    StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|-------------------------------------------- |----------:|---------:|----------:|------------:|------------:|------------:|--------------------:|
|           ConcatSpansStringBuilderBenchmark | 127.27 ns | 2.190 ns | 2.0487 ns |      0.0966 |           - |           - |               304 B |
|                  ConcatSpansCopyToBenchmark | 105.94 ns | 2.125 ns | 2.2736 ns |      0.0813 |           - |           - |               256 B |
|       ConcatSpansFastStringBuilderBenchmark | 124.64 ns | 1.844 ns | 1.5402 ns |      0.0813 |           - |           - |               256 B |
| ConcatSpansFastUnsafeStringBuilderBenchmark |  66.93 ns | 1.045 ns | 0.9772 ns |      0.0407 |           - |           - |               128 B |
