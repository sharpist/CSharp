using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        ConcurrentQueue<int> cq = new ConcurrentQueue<int>();

        for (int i = 0; i < 1000; i++) cq.Enqueue(i);


        int result = 0;
        Action action = () =>
        {
            int value, sum = 0;
            while (cq.TryDequeue(out value)) sum += value; // извлечение и суммирование значений

            Interlocked.Add(ref result, sum); // потокобезопасная операция присвоения результата
        };
        // начать 4 одновременных действия
        Parallel.Invoke(action, action, action, action);

        Console.WriteLine($"result = {result}"); // 499500
    }
}