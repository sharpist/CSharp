using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var data = new List<Person>
        {
            new Person(3, "Александр"), new Person(2, "Катерина"),
            new Person(4, "Андрей"),    new Person(1, "Наталья"),
            new Person(6, "Ольга"),     new Person(5, "Дмитрий")
        };

        // отношение ключа к делегату
        var keyset = new Dictionary<string, Func<Person, object>>
        {
            { "Id", x => x.Id }, { "Name", x => x.Name }
        };
        var key = "Id";                    // опорное значение упорядочивания

        Func<Person, object> func;         // делегат
        keyset.TryGetValue(key, out func); // инициализировать делегат по ключу
        var result = Sort(data, func);     // упорядочить

        result?.ForEach(x => Console.WriteLine($"Id: {x.Id}, Name: {x.Name}"));
        /* Output: ordered by "Id"
            Id: 1, Name: Наталья
            Id: 2, Name: Катерина
            Id: 3, Name: Александр
            Id: 4, Name: Андрей
            Id: 5, Name: Дмитрий
            Id: 6, Name: Ольга
        */
    }
    // функция упорядочивания
    static List<Person> Sort(List<Person> data, Func<Person, object> func)
        => data.OrderBy(func).ToList();
}

struct Person
{
    public Int32 Id { get; }
    public String Name { get; }

    public Person(int Id, string Name) => (this.Id, this.Name) = (Id, Name);
}