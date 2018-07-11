using System.Threading;
using static System.Console;
using static System.Threading.Interlocked;

class Program
{
    static void Main()
    {
        var ts = new ThreadSafe();

        var threads = new Thread[5]; // создать 5 потоков, которые вызывают
        for (int i = 0; i < 5; i++) // метод AddToTotal на одном!!! и том же экземпляре ThreadSafe
        {
            threads[i] = new Thread(new ThreadStart(() => ts.AddToTotal(10)));
            threads[i].Name = string.Format($"Работает {i + 1} поток");
        }
        foreach (Thread t in threads) // запуск всех потоков
            t.Start();
    }
}

class ThreadSafe
{
    // содержит значение, которое может обновляться разными потоками
    // защищается от несинхронизированного доступа
    private double total;


    public double AddToTotal(double addend)
    {
        double initial, computed;
        do
        {
            initial  = total;

            computed = initial + addend;
        }
        // если ни один поток не обновил общую сумму, тогда
        // total и initial равны, когда CompareExchange их сравнивает,
        // и computed копируется в total
        while (initial != CompareExchange(ref total, computed, initial));
        // CompareExchange возвращает значение, которое было в total
        // перед обновлением, которое равно initial, поэтому цикл завершается

        /// информация о потоке
        WriteLine($"{Thread.CurrentThread.Name}: {total - addend} + {addend} = {computed}");
        return computed;
    }
}
/*
Работает 1 поток: 0 + 10 = 10
Работает 3 поток: 20 + 10 = 30
Работает 2 поток: 10 + 10 = 20
Работает 4 поток: 30 + 10 = 40
Работает 5 поток: 40 + 10 = 50
всегда верный результат
*/
