using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PLINQ
{
    class Program
    {
        public static void Main() => buildRequest();

        // 1.      "Land Cruiser" Toyota           (Foundation 1937, Toyota Aichi Japan)
        // 2.      "X5" BMW                (Foundation 1916, Munich Bavaria Germany)
        // 3.      "Corvette C7" Chevrolet         (Foundation 1911, Detroit Michigan U.S.)
        // 4.      "Santa Fe" Hyundai              (Foundation 1967, Seoul South Korea)
        // 5.      "Giulia" Alfa Romeo             (Foundation 1910, Turin Piedmont Italy)

        private static void buildRequest()
        {   // оба источника данных должны быть объектами типа IEnumerable или ParallelQuery
            var parallelQuery = from c in Automaker.Cars      .AsParallel()
                                join d in FoundationDate.Dates.AsParallel()

                                /// метод Split разбивает строки на массив подстрок (по запятым)
                                on c.Split(',')[0] equals d.Split(',')[1]
                                            /// создать новый источник данных на основе двух
                                            select new Request
                                            {
                                                Id = Convert.ToInt32(d.Split(',')[0]),
                                                Model  = c.Split(',')[1],
                                                Brand  = c.Split(',')[2],
                                                Office = c.Split(',')[3],
                                                Date   = Convert.ToDateTime(d.Split(',')[2], new CultureInfo("en-US"))
                                            };

            // вывести результаты
            var results = new List<Request>(parallelQuery.OrderBy(x => x.Id)); // упорядочить по Id
            results?.ForEach(x => Console.Write($"{x.Id}.\t\"{x.Model}\" {x.Brand}\t\t(Foundation {x.Date.Year}, {x.Office})\n"));
        }
    }

    struct Request
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Office { get; set; }
        public DateTime Date { get; set; }
    }

    // источники данных
    public struct Automaker
    {
        public static string[] Cars = {
            "0T5R,Giulia,Alfa Romeo,Turin Piedmont Italy",
            "20MA,Land Cruiser,Toyota,Toyota Aichi Japan",
            "0DK7,Corvette C7,Chevrolet,Detroit Michigan U.S.",
            "03HL,X5,BMW,Munich Bavaria Germany",
            "V01C,Santa Fe,Hyundai,Seoul South Korea"
        };
    }
    public struct FoundationDate
    {
        public static string[] Dates = {
            "5,0T5R,Jun 24 1910 00:00AM",
            "1,20MA,Aug 28 1937 00:00AM",
            "3,0DK7,Nov  3 1911 00:00AM",
            "2,03HL,Mar  7 1916 00:00AM",
            "4,V01C,Dec 29 1967 00:00AM"
        };
    }
}