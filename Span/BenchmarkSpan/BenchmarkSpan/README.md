# Benchmark Span

## Benchmark:

![benchmark](benchmark.gif)

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
AMD Phenom(tm) II X4 945 Processor, 1 CPU, 4 logical and 4 physical cores
  [Host]     : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3132.0
  DefaultJob : .NET Framework 4.6.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3132.0


```
|    Method |    StdDev |       Min |       Max |    Median | Iterations |
|---------- |----------:|----------:|----------:|----------:|-----------:|
| Substring | 0.6681 ns | 124.17 ns | 126.52 ns | 125.11 ns |      14.00 |
|      Span | 0.3999 ns |  71.30 ns |  72.46 ns |  72.12 ns |      15.00 |
