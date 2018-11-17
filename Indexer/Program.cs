using IndexersSamples.SampleOne;
using System;
using static System.Console;

namespace IndexersSamples
{
    class Program
    {
        static void SampleOne()
        {
            var store = new DataSamples(500000); // создать хранилище

            var sample = store[3];               // сгенерировать первую страницу и извлечь элемент по индексу
            WriteLine(sample.Temp);

            store[80] = sample;                  // обновить, произвести работу на первой странице данных
            WriteLine(store[80].Temp);           // теперь страница считается грязной


            var sample2 = store[2020];           // сгенерировать вторую страницу
            WriteLine(sample2.Temp);

            store[3547] = sample2;               // произвести работу на третьей странице
            WriteLine(store[3547].Temp);


            // убедиться, что страницы проходят и выходят из памяти
            for (int i = 5; i < 500000; i += 1100)
                WriteLine(store[i].Pressure);


            try
            {
                var sample3 = store[1000000];    // аргумент за пределами допустимого диапазона
            }
            catch (IndexOutOfRangeException e)
            {
                WriteLine(e.Message);            // перехвачено исключение
            }
        }
        static void Main() => SampleOne();
    }
}