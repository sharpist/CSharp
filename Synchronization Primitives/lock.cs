using System;
using System.Threading;
using static System.Console;

class MyTheard
{
    // закрытый объект для блокировки
    private object threadLock = new object();

    public void ThreadNumbers()
    {
        lock (threadLock) // маркер блокировки — указанный объект
        {
            WriteLine($"{Thread.CurrentThread.Name}" + "\n" // информация о потоке
                + "Используется метод ThreadNumbers");

            Write("Числа: ");
            var r = new Random();
            for (int i = 1; i <= 10; i++) // вывод чисел
            {
                Thread.Sleep(500 * r.Next(5)); // пауза
                Write(i + (i<10?", ":"\n\n"));
            }
        }
    }
}

class Program
{
    static void Main()
    {
        /// Первичный поток начинает своё существование, порождая 5 вторичных рабочих потоков.
        /// Каждый такой поток вызывает метод ThreadNumbers на одном!!! и том же экземпляре MyTheard.
        /// Как только потоки требуют от MyTheard печати числовых данных, планировщик потоков может менять их местами
        /// (этому способствует созданная задержка в задании).
        /// Поэтому требуется блокировать участок кода в методе ThreadNumbers для согласованного вывода результатов.
        var mt = new MyTheard();

        var threads = new Thread[5]; // создать 5 потоков
        for (int i = 0; i < 5; i++)
        {
            threads[i] = new Thread(new ThreadStart(mt.ThreadNumbers));
            threads[i].Name = string.Format($"Работает {i+1} поток");
        }

        foreach (Thread t in threads) // запуск всех потоков
            t.Start();
    }
}
/*
Работает 1 поток
Используется метод ThreadNumbers
Числа: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10

Работает 3 поток
Используется метод ThreadNumbers
Числа: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10

Работает 2 поток
Используется метод ThreadNumbers
Числа: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10

Работает 4 поток
Используется метод ThreadNumbers
Числа: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10

Работает 5 поток
Используется метод ThreadNumbers
Числа: 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
*/
