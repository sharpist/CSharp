using System;
using System.Threading.Tasks;
using static System.Console;

public class Example
{
    private static async Task DoMultipleAsync()
    {
        Task task1 = ExcAsync(info: "First Task");
        Task task2 = ExcAsync(info: "Second Task");
        Task task3 = ExcAsync(info: "Third Task");
        Task tasks = Task.WhenAll(task1, task2, task3);

        try
        {
            await tasks;
        }
        catch (Exception ex)
        {
            WriteLine($"Exception: {ex.Message}");
            WriteLine($"Task IsFaulted: {tasks.IsFaulted}");

            foreach (var inEx in tasks.Exception.InnerExceptions)
                WriteLine($"Task Inner Exception: {inEx.Message}");
        }

        async Task ExcAsync(string info) // local async function
        {
            await Task.Delay(100);
            throw new Exception($"Error-{info}");
        }
    }
    // Exception: Error-First Task
    // Task IsFaulted: True
    // Task Inner Exception: Error-First Task
    // Task Inner Exception: Error-Second Task
    // Task Inner Exception: Error-Third Task

    public static void Main() => DoMultipleAsync().Wait();
}
