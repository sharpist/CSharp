using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
///using System;

namespace BenchmarkSpan
{
    public class Program
    {
        public static void Main()
        {
            Summary result = BenchmarkRunner.Run<Benchmark>();
            ///Console.WriteLine("{0} {1}", result.Benchmarks[index], result.Reports[index]);
        }
    }

    // локальная конфигурация
    internal class MyConfig : ManualConfig
    {
        public MyConfig()
        {
            Add(TargetMethodColumn.Method,
                StatisticColumn.Min,
                StatisticColumn.Max,
                StatisticColumn.Median,
                StatisticColumn.StdDev,
                StatisticColumn.Iterations); // выбрать статистику

            Add(HtmlExporter.Default, CsvExporter.Default, MarkdownExporter.GitHub); // сгенерировать html, csv, markdown

            Add(ConsoleLogger.Default); // вывод в консоль

            Add(EnvironmentAnalyser.Default);

            Add(ExecutionValidator.FailOnError);

            UnionRule = ConfigUnionRule.AlwaysUseLocal; // не применять стандартный конфиг
        }
    }
}
