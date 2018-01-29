using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static System.Console;


class Program
{
    static void Main()
    {
        const int numberTasks = 2;
        const int partitionSize = 1000000;

        // подготовить данные с методом FillData
        var data = new List<string>(FillData(partitionSize * numberTasks));
        // создать синхронизатор
        var barrier = new Barrier(numberTasks + 1); // 2 задачи + метод Main()


        // создать 2 задачи
        var taskFactory = new TaskFactory();
        var tasks = new Task<int[]>[numberTasks];
        for (int i = 0; i < numberTasks; i++)
        {
            tasks[i] = taskFactory.StartNew<int[]>(CalculationInTask, Tuple.Create(i, partitionSize, barrier, data));
        }
        // запуск задач на исполнение
        ///метод Main() сигнализирует о завершении и ожидает, пока все участники просигнализируют о завершении, либо удалят себя как участников из Barrier
        barrier.SignalAndWait();


        // все возвращённые результаты объединяются вместе
        var resultCollection = tasks[0].Result.Zip(tasks[1].Result, (c1, c2) => { return c1 + c2; });
    }


    // создаёт и заполняет коллекцию
    public static IEnumerable<string> FillData(int size)
    {
        var r    = new Random();
        var data = new List<string>(size);

        for (int i = 0; i < size; i++) data.Add(GetString(r));
        return data;
    }
    // возвращает строки из произвольных значений
    private static string GetString(Random r)
    {
        var sb = new StringBuilder(6);

        for (int i = 0; i < 6; i++) sb.Append((char)(r.Next(26) + 97));
        return sb.ToString();
    }


    // задание подсчёта количества строк для каждой из задач
    private static int[] CalculationInTask(object p)
    {
        var p1 = p as Tuple<int, int, Barrier, List<string>>; // quadruple кортеж
        // извлекает элементы кортежа
        int start         = p1.Item1 * p1.Item2;
        int end           = start + p1.Item2;
        Barrier barrier   = p1.Item3;
        List<string> data = p1.Item4;


        WriteLine($"Задача {Task.CurrentId} проходит коллекцию от {start}\tдо {end}");


        // подсчитывает количество строк по первым символам
        int[] charCount = new int[26]; // воплощает импровизированные позиции символов в порядке алфавита
        for (int i = start; i < end; i++)
        {
            char c = data[i][0]; // i элемент коллекции, 0 первый символ строки
            charCount[c - 97]++; // [c - 97] вычисляет позицию символа, incr суммирует показатель
        }


        WriteLine($"\nЗадача {Task.CurrentId} завершила вычисление: 'a' = {charCount[0]}, 'z' = {charCount[25]}"); // 0 = a, 1 = b ... 25 = z
        barrier.RemoveParticipant(); // удалить задачу из Barrier после выполнения задания
        WriteLine($"Задача {Task.CurrentId} удалена; количество оставшихся участников: {barrier.ParticipantsRemaining}");
        return charCount;
    }
}
/*
Задача 1 проходит коллекцию от 0        до 1000000
Задача 2 проходит коллекцию от 1000000  до 2000000

Задача 2 завершила вычисление: 'a' = 38544, 'z' = 38419
Задача 2 удалена; количество оставшихся участников: 1

Задача 1 завершила вычисление: 'a' = 38323, 'z' = 38349
Задача 1 удалена; количество оставшихся участников: 1
*/
