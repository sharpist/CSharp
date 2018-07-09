using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

public class Example
{
    public static void Main()
    {
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;


        var files = new List<(string, string, long, DateTime)>();

        var task = Task.Run(() =>
        {
            var dir = "C:\\Users\\***\\Test";
            var obj = new Object(); // lock

            if (Directory.Exists(dir))
            {
                Parallel.ForEach(Directory.GetFiles(dir),
                f =>
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    var fi = new FileInfo(f);
                    lock (obj) {
                        files.Add((fi.Name,
                                   fi.DirectoryName,
                                   fi.Length,
                                   fi.LastWriteTimeUtc));
                    }
                });
            }
        }, token);

        //tokenSource.Cancel();

        try
        {
            task.Wait();
            WriteLine($"Retrieved information for {files.Count} files.");
        }

        catch (AggregateException e)
        {
            WriteLine("Exception messages:");

            foreach (var ie in e.InnerExceptions)
                WriteLine($"   {ie.GetType().Name}: {ie.Message}");

            WriteLine($"\nTask status: {task.Status}");
        }

        finally
        { tokenSource.Dispose(); }
    }
}
// Retrieved information for 5 files.

/// OR IF INITIATED tokenSource.Cancel();

/// Exception messages:
///    TaskCanceledException: Отменена задача.
///
/// Task status: Canceled
