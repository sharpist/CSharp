using System;
using System.Threading;
using System.Threading.Tasks;


class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Main() =>");
        mainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        Console.WriteLine("Main() end");
    }
    private static async Task mainAsync(string[] args)
    {
        Console.WriteLine("mainAsync() =>");
        await Task.Run(() => createAndRunThread());
        Console.WriteLine("mainAsync() end");
    }




    private static void createAndRunThread()
    {
        Console.WriteLine("createAndRunThread() =>\n");
        var s = new SemaphoreSlim(0, 3); // synchronizer

        var threads = new Thread[3];
        for (int i = 0; i < 3; i++)
        {
            threads[i] = new Thread(new ThreadStart(() =>
            {
                s.Wait(); // synchronizer
                longOperation(s);
            }));
            threads[i].Name = string.Format($"Thread #{i+1}");
        }
        foreach (Thread t in threads) t.Start();

        s.Release(1); // synchronizer
        Console.WriteLine("createAndRunThread() end");
    }


    private static void longOperation(SemaphoreSlim s)
    {
        Thread.Sleep(250);
        Console.WriteLine($"\n{Thread.CurrentThread.Name} works...");
        
        var r = new Random();

        for (int i = 1; i <= 10; i++)
        {
            Thread.Sleep(250 * r.Next(5));
            Console.Write(i + (i<10?", ":"\n"));
        }

        s.Release(); // synchronizer
        Console.WriteLine($"{Thread.CurrentThread.Name} finished!");
    }
}
/*
Main() =>
mainAsync() =>
createAndRunThread() =>

createAndRunThread() end
mainAsync() end
Main() end

Thread #3 works...
1, 2, 3, 4, 5, 6, 7, 8, 9, 10
Thread #3 finished!

Thread #2 works...
1, 2, 3, 4, 5, 6, 7, 8, 9, 10
Thread #2 finished!

Thread #1 works...
1, 2, 3, 4, 5, 6, 7, 8, 9, 10
Thread #1 finished!
*/

