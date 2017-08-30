...
    class Program
    {
        ...
            Cast<Person> personnel = new Cast<Person>(
                new Person { ID = 1, Age = 34, Name = "John" },
                new Person { ID = 2, Age = 30, Name = "Fred" },
                new Person { ID = 3, Age = 27, Name = "Elysia" },
                new Person { ID = 4, Age = 23, Name = "April" },
                new Person { ID = 5, Age = 31, Name = "Alexander" });

            Person match = personnel.Find((Person p) => { return p.ID == 4; });

            Console.WriteLine($"Name: {match.Name}, Age: {match.Age}");
            // Name: April, Age: 23
    }
    
    class Cast<T>
    {
        private T match;              // результат
        private T[] arr { get; set; } // массив
        public Cast(params T[] p)     // активный конструктор
        {
            this.arr = new T[p.Length];

            for (int i = 0; i < p.Length; i++)
            { this.arr[i] = p[i]; }
        }

        public T Find(Func<T, bool> op)
        {
            foreach (var value in this.arr)
            {
                bool checkValue = op(value);

                if (checkValue == true)
                {
                    this.match = value;
                    break;
                }
            }
            return this.match;
        }
    }

    struct Person
    {
        public int ID { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
    }
