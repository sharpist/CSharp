using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Columns;
///using BenchmarkDotNet.Order;
using System;

namespace BenchmarkSpan
{
    [RankColumn]
    ///[Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [Config(typeof(MyConfig))]
    public class Benchmark
    {
        const string fullName = "Sam Jordan Freeman";
        static readonly NameParser parser = new NameParser();


        // взять за нормаль для относительного времени
        [Benchmark(Description = "Substring", Baseline = true)]
        public void GetLastNameUsingSubstring()
        {
            ///String resultWithSubstring =
                parser.GetLastNameUsingSubstring(fullName);
            ///Console.WriteLine(resultWithSubstring); // Freeman
        }

        [Benchmark(Description = "Span")]
        public void GetLastNameUsingSpan()
        {
            ///ReadOnlySpan<char> resultWithSpan =
                parser.GetLastNameUsingSpan(fullName.ToCharArray());
            ///Console.WriteLine(resultWithSpan.ToString()); // Freeman
        }
    }
}
