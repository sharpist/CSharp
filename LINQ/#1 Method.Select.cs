using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var team = new Team<Worker> {
                new Worker { ID = 1, Name = "Erik", Age = 35, City = "Austin" },
                new Worker { ID = 2, Name = "Sean", Age = 26, City = "Miami" },
                new Worker { ID = 3, Name = "Alex", Age = 30, City = "Los Angeles" },
                new Worker { ID = 4, Name = "John", Age = 29, City = "Rochester" },
                new Worker { ID = 5, Name = "Kate", Age = 23, City = "San Francisco" }
            };

            // Example 1
            IEnumerable<string> names = team.Select((Worker w) => w.Name);

            foreach (string name in names)
            {
                Console.WriteLine(name);
            }
            // Will show all employee names

            // Example 2
            IEnumerable<NewType> workers = team.Select( (Worker w) =>
            new NewType { Name = w.Name, ID = w.ID, City = w.City } );

            foreach (NewType worker in workers)
            {
                Console.WriteLine(
                $"ID {worker.ID}: {worker.Name} from {worker.City}" );
            }

            // Example 3
            IEnumerable<Worker> workers2 = from Worker worker in team
                                          where worker.Age <= 30
                                          select worker;

            foreach (Worker worker in workers2)
            {
                Console.WriteLine($"{worker.Name} from {worker.City}");
            }
            /*
            Sean from Miami
            Alex from Los Angeles
            John from Rochester
            Kate from San Francisco
            */
        }
    }

    class Team<T> : IEnumerable<T> // enumerable collection
    {
        private T[] workers;
        private const byte DEFAULTSIZE = 5;
        public Team() {
            this.workers = new T[DEFAULTSIZE];
        }

        private int i = 0;
        public void Add(T param)
        {
            this.workers[i++] = param;
            i %= workers.Length;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (T worker in this.workers)
            {
                yield return worker;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    struct Worker
    {
        public int ID;
        public int Age;
        public string Name;
        public string City;
    }

    class NewType // for output data
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
    }

    public static class Ext
    {
        public static IEnumerable<TResult> Select<TSource, TResult> (
            this IEnumerable<TSource> source,
            Func<TSource, TResult> selector ) // extension method
        {
            List<TResult> subset = new List<TResult>();
            foreach (var item in source)
            {
                subset.Add(selector(item));
            }
            return subset;
        }
    }
}
