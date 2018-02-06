using System;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Threading.Thread;


class Program
{
    public static void Main(string[] args)
    {
        WriteLine("Main() =>"                        + $"\t\t\tid{CurrentThread.ManagedThreadId}");
        mainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        WriteLine("Main() end"                       + $"\t\t\tid{CurrentThread.ManagedThreadId}");
    }
    private static async Task mainAsync(string[] args)
    {
        WriteLine("mainAsync() =>"                   + $"\t\t\tid{CurrentThread.ManagedThreadId}");
        await Task.Run(() => createThreads());
        WriteLine("mainAsync() end"                  + $"\t\t\tid{CurrentThread.ManagedThreadId}");
    }




    private static void createThreads()
    {
        WriteLine("createThreads() =>"               + $"\t\tid{CurrentThread.ManagedThreadId}\n");
        var s = new SemaphoreSlim(0, 3); // synchronizer

        var threads = new Thread[3];

        for (int i = 0; i < 3; i++) {
            threads[i] = new Thread(new ThreadStart(() => {
                s.Wait(); // synchronizer
                longOperation(s);
            }));
            threads[i].Name = string.Format($"Thread #{i+1}");
        }
        foreach (Thread t in threads) t.Start();

        WriteLine("createThreads() end"              + $"\t\tid{CurrentThread.ManagedThreadId}");
        s.Release(1); // synchronizer
    }


    private static void longOperation(SemaphoreSlim s)
    {
        WriteLine($"\n{CurrentThread.Name} works..." + $"\t\tid{CurrentThread.ManagedThreadId}");

        var r = new Random();
        for (int i = 1; i <= 10; i++) {
            Sleep(250 * r.Next(5));
            Write(i + (i<10?", ":"\n"));
        }

        WriteLine($"...finished!");
        s.Release(); // synchronizer
    }
}
/*
Main() =>                       id1
mainAsync() =>                  id1
createThreads() =>              id3

createThreads() end             id3
mainAsync() end                 id3
Main() end                      id1

Thread #1 works...              id5
1, 2, 3, 4, 5, 6, 7, 8, 9, 10
...finished!

Thread #2 works...              id6
1, 2, 3, 4, 5, 6, 7, 8, 9, 10
...finished!

Thread #3 works...              id7
1, 2, 3, 4, 5, 6, 7, 8, 9, 10
...finished!
*/
