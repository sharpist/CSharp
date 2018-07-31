using System.Collections;
using System.Collections.Generic;
using static System.Console;

namespace ExampleProject
{
    class RudimentaryMultiValuedDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, List<TValue>>>
    {
        private Dictionary<TKey, List<TValue>> internalDictionary = new Dictionary<TKey, List<TValue>>();

        public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetEnumerator()
            => internalDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => internalDictionary.GetEnumerator();

        // пользовательская реализация индексатора
        // применяется для инициализации коллекции с помощью индексов
        public List<TValue> this[TKey key]
        {
            get => internalDictionary[key];
            set => Add(key, value);
        }


        public void Add(TKey key, params TValue[] values)
            => Add(key, (IEnumerable<TValue>)values);

        public void Add(TKey key, IEnumerable<TValue> values)
        {
            if (!internalDictionary.TryGetValue(key, out List<TValue> storedValues))
                internalDictionary.Add(key, storedValues = new List<TValue>());

            storedValues.AddRange(values);
        }
    }


    class Program
    {
        static void Main()
        {
            var rudimentaryMultiValuedDictionary1 = new RudimentaryMultiValuedDictionary<string, string>()
                {
                    {"Group1", "Bob", "John", "Mary" },
                    {"Group2", "Eric", "Emily", "Debbie", "Jesse" }
                };
            var rudimentaryMultiValuedDictionary2 = new RudimentaryMultiValuedDictionary<string, string>()
                {
                    ["Group1"] = new List<string>() { "Bob", "John", "Mary" },
                    ["Group2"] = new List<string>() { "Eric", "Emily", "Debbie", "Jesse" }
                };


            WriteLine("Использование первого многозначного словаря,\nсозданного с помощью инициализатора коллекции:");
            foreach (KeyValuePair<string, List<string>> group in rudimentaryMultiValuedDictionary1)
            {
                WriteLine($"\r\nУчастники группы {group.Key}: ");

                foreach (string member in group.Value)
                    WriteLine(member);
            }

            WriteLine("\r\nИспользование второго многозначного словаря,\nсозданного с помощью инициализатора коллекции\nс использованием индексации:");
            foreach (var group in rudimentaryMultiValuedDictionary2)
            {
                WriteLine($"\r\nУчастники группы {group.Key}: ");

                foreach (string member in group.Value)
                    WriteLine(member);
            }
        }
        /*
        Использование первого многозначного словаря,
        созданного с помощью инициализатора коллекции:

        Участники группы Group1:
        Bob
        John
        Mary

        Участники группы Group2:
        Eric
        Emily
        Debbie
        Jesse

        Использование второго многозначного словаря,
        созданного с помощью инициализатора коллекции
        с использованием индексации:

        Участники группы Group1:
        Bob
        John
        Mary

        Участники группы Group2:
        Eric
        Emily
        Debbie
        Jesse
        */
    }
}