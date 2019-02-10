using ExtensionMethods; /* !!! */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

public class Program
{
    public static void Main()
    {
        Worker[] workers = {
            new Worker { Name = "Александр", Rank = 5, Salary = 11.000M },
            new Worker { Name = "Анастасия", Rank = 3, Salary = 9.000M },
            new Worker { Name = "Дмитрий",   Rank = 5, Salary = 11.000M },
            new Worker { Name = "Татьяна",   Rank = 2, Salary = 8.000M },
            new Worker { Name = "Юрий",      Rank = 4, Salary = 1.0000M },
            new Worker { Name = "Катерина" }};

        var filterByRank = workers.FilterByRank(r => r.Rank <= 3);
        var totalSalary  = filterByRank.TotalSalary();

        filterByRank.ToList().ForEach(worker => Console.WriteLine(worker.Name));
        Console.WriteLine($"\nОбщая зарплата: {totalSalary:C3}");
        /*
            Анастасия
            Татьяна

            Общая зарплата: 17,000 ?
        */
    }
}

public class Worker
{
    public string   Name   { get; set; }
    public byte?    Rank   { get; set; }
    public decimal? Salary { get; set; }
}


namespace ExtensionMethods
{
    public static class MyExtensions
    {
        // метод расширения интерфейса
        public static decimal TotalSalary(this IEnumerable<Worker> workers)
        {
            var total = 0M;
            foreach (var worker in workers)
                total += worker?.Salary ?? 0;
            return total;
        }

        // фильтрующий расширяющий метод
        public static IEnumerable<Worker> FilterByRank(
            this IEnumerable<Worker> workers, Func<Worker, bool> predicate)
        {
            foreach (var worker in workers)
                if (predicate(worker))
                    yield return worker;
        }
    }
}