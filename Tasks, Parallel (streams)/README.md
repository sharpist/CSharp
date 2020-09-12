## Задачи и класс Task
____

Библиотека параллельных задач **TPL** *(Task Parallel Library)* служит для распараллеливания задач
и одновременного их выполнения сразу на всех доступных ядрах процессора.
В *.NET* задача представлена специальным классом – **```Task```**, который находится в пространстве имен:

```csharp
using System.Threading.Tasks;
```

> Данный класс описывает отдельную задачу, которая запускается асинхронно в одном из потоков
из пула потоков. Хотя её также можно запускать синхронно в текущем потоке.

Способы определения и запуска задач:

```csharp
public static void Main(string[] args)
{
    var task1 = new Task(() => {
        // здесь делаем что-то...
        Console.WriteLine($"Task Id: {Task.CurrentId}");
    });
    task1.Start();

    var task2 = Task.Run(() => {
        // здесь делаем что-то...
        Console.WriteLine($"Task Id: {Task.CurrentId}");
    });

    var task3 = Task.Factory.StartNew(() => {
        // здесь делаем что-то...
        Console.WriteLine($"Task Id: {Task.CurrentId}");
    });

    Task.WaitAll(task1, task2, task3);

    /* Output:
        Task Id: 3
        Task Id: 1
        Task Id: 2
    */
}
```

***1.*** **Задачи не выполняются последовательно**, первая запущенная задача может завершиться после
последней задачи.

***2.*** **Одна задача может запускать другую – вложенную задачу**. При этом эти задачи выполняются
независимо друг от друга. Причём несмотря на ожидание выполнения внешней задачи, вложенная
задача может завершить выполнение даже после завершения метода Main:

```csharp
public static void Main(string[] args)
{
    // внешняя задача
    var outer = Task.Factory.StartNew(() => {
        Console.WriteLine("Outer task starting...");
        // вложенная задача
        var inner = Task.Factory.StartNew(() => {
            Console.WriteLine("Inner task starting...");
            Thread.Sleep(2000);
            Console.WriteLine("Inner task finished");
        });
        Console.WriteLine("Outer task finished");
    });
    outer.Wait(); // ожидать выполнение внешней задачи
    Console.WriteLine("End of Main");

    /* Output:
        Outer task starting...
        Outer task finished
        End of Main
        Inner task starting...
    */
}
```

***3.*** Для того, чтобы вложенная задача выполнялась вместе с внешней, необходимо использовать
значение **```TaskCreationOptions.AttachedToParent```**:

```csharp
public static void Main(string[] args)
{
    // внешняя задача
    var outer = Task.Factory.StartNew(() => {
        Console.WriteLine("Outer task starting...");
        // вложенная задача
        var inner = Task.Factory.StartNew(() => {
            Console.WriteLine("Inner task starting...");
            Thread.Sleep(2000);
            Console.WriteLine("Inner task finished");
        }, TaskCreationOptions.AttachedToParent);
        Console.WriteLine("Outer task finished");
    });
    outer.Wait(); // ожидать выполнение внешней задачи
    Console.WriteLine("End of Main");

    /* Output:
        Outer task starting...
        Outer task finished
        Inner task starting...
        Inner task finished
        End of Main
    */
}
```

***4.*** **Задачи могут** не только выполняться как процедуры, но и **возвращать определённые результаты**:

* Чтобы задать возвращаемый из задачи тип, необходимо типизировать **```Task<int>```** – в
данном случае задача будет возвращать значение **```int```**.

* В качестве задачи должен выполняться метод, возвращающий данный тип.

* Ответ будет храниться в свойстве **```Result```**.

* ==При обращении к свойству **```Result```** программа текущий поток останавливает== и ждёт,
когда будет получен результат из выполняемой задачи.

```csharp
public static void Main(string[] args)
{
    byte n = 10;
    var task = Task<ulong>.Factory.StartNew(() => {
        ulong factorial = 1;

        while (n > 0) factorial *= n--;

        return factorial;
    });
    // ожидать получение результата
    Console.WriteLine($"Факториал числа {n} равен: {task.Result}");

    /* Output:
        Факториал числа 10 равен: 3628800
    */
}
```

***5.*** **Существуют задачи продолжения, способные выполняться после завершения других задач**.
Такие задачи задаются с помощью метода **```ContinueWith```**, который принимает делегат **```Action<Task>```**,
поэтому передаваемый в качестве значения параметра метод должен получать параметр типа **```Task```**,
благодаря этому запрашиваются различные свойства предыдущей задачи:

```csharp
public static void Main(string[] args)
{
    var task1 = new Task(() => {
        Console.WriteLine($"Id начальной задачи:\t{Task.CurrentId}");
    });
    // задача продолжения
    var task2 = task1.ContinueWith(Display);

    task1.Start();
    // ждем окончания второй задачи
    task2.Wait();

    Console.WriteLine("Выполняется метод Main...");

    /* Output:
        Id начальной задачи:    2
        Id продолжающей задачи: 1
        Id предыдущей задачи:   2
        Выполняется метод Main...
    */
}

private static void Display(Task task)
{
    Console.WriteLine($"Id продолжающей задачи:\t{Task.CurrentId}");
    Console.WriteLine($"Id предыдущей задачи:\t{task.Id}");
    Thread.Sleep(2000);
}
```

Также возможно передать результат работы предыдущей задачи:

```csharp
public static void Main(string[] args)
{
    var task1 = new Task<double>(() => {
    return Op<int, int, double>(
        (a, b) => a * b, 3, 9);
    });
    // задача продолжения
    var task2 = task1.ContinueWith(t => Display(t.Result));

    task1.Start();
    // ждем окончания второй задачи
    task2.Wait();

    Console.WriteLine("Выполняется метод Main...");

    /* Output:
        Результат выполнения начальной задачи: 27
        Выполняется метод Main...
    */
}

private static TResult Op<T1, T2, TResult>(
    Func<T1, T2, TResult> f, T1 arg1, T2 arg2) => f(arg1, arg2);

private static void Display<T>(T res) =>
    Console.WriteLine($"Результат выполнения начальной задачи: {res}");
```
