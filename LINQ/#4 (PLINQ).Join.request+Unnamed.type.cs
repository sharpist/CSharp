using System;
using System.Linq;

namespace Example
{
    class Program
    {
        public static void Main()
        {
            // data sources
            string[] companyNames = {
                "3:A030:MTS",
                "1:050F:Beeline",
                "2:10D0:Megafon"
                //...
            };

            string[] companyOffices = {
                "050F:Yaroslavl, Republican street 3/1",
                "10D0:Yaroslavl, Pervomaiskaya Street 39/10",
                "A030:Yaroslavl, Kirova street 10"
                //...
            };

            // PLINQ-request
            var request = from n in companyNames  .AsParallel()
                          join o in companyOffices.AsParallel()
                          on n.Split(':')[1] equals o.Split(':')[0]
                          select new {
                              id = n.Split(':')[0],
                              name = n.Split(':')[2],
                              office = o.Split(':')[1]
                          };

            // display
            request.OrderBy(x => x.id).ToList()
                .ForEach(x => Console.Write($"{x.id}.   {x.name}\t{x.office}\n"));
        }
        // 1.   Beeline    Yaroslavl, Republican street 3/1
        // 2.   Megafon    Yaroslavl, Pervomaiskaya Street 39/10
        // 3.   MTS        Yaroslavl, Kirova street 10

    }
}