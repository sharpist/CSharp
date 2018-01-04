using System;
using System.Collections.Generic;
using System.Linq;

namespace Slide
{
    class Program
    {
        public static void Main()
        {
            // отношение ключа к делегату
            var keys = new Dictionary<string, Func<Person, object>>()
            {
                { "Id",   x => x.Id },
                { "Name", x => x.Name }
            };

            // данные
            var dataList = new List<Person>()
            {
                new Person() { Id = "3", Name = "Alexander"},
                new Person() { Id = "2", Name = "Katerina"},
                new Person() { Id = "4", Name = "Andrey"},
                new Person() { Id = "1", Name = "Natalia"}
            };


            // выбранный ключ "Id" - опорное значение для упорядочивания
            string property = "Id";


            Func<Person, object> func;               // объявление делегата
            keys.TryGetValue(property, out func);    // инициализация делегата по ключу

            var sortedResult = Sort(dataList, func); // теперь выбран верный делегат


            sortedResult?.ForEach(x => Console.WriteLine($"Id = {x.Id}, Name = {x.Name}"));
            /* упорядочено по "Id"

               Id = 1, Name = Natalia
               Id = 2, Name = Katerina
               Id = 3, Name = Alexander
               Id = 4, Name = Andrey
            */
        }


        // функция упорядочивания
        public static List<Person> Sort(List<Person> dataList, Func<Person, object> p)
            => dataList.OrderBy(p).ToList();
    }

    struct Person
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}