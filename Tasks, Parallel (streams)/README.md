
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
    var task2 = task1.ContinueWith(display);

    task1.Start();
    // ждём окончания второй задачи
    task2.Wait();

    Console.WriteLine("Выполняется метод Main...");

    /* Output:
        Id начальной задачи:    2
        Id продолжающей задачи: 1
        Id предыдущей задачи:   2
        Выполняется метод Main...
    */
}

private static void display(Task task)
{
    Console.WriteLine($"Id продолжающей задачи:\t{Task.CurrentId}");
    Console.WriteLine($"Id предыдущей задачи:\t{task.Id}");
    Thread.Sleep(2000);
}
```

> В задачу продолжения можно передавать результат работы предыдущей задачи.

```csharp
public static void Main(string[] args)
{
    var task1 = new Task<double>(() => {
    return op<int, int, double>(
        (a, b) => a * b, 3, 9);
    });
    // задача продолжения
    var task2 = task1.ContinueWith(t => display(t.Result));

    task1.Start();
    // ждём окончания второй задачи
    task2.Wait();

    Console.WriteLine("Выполняется метод Main...");

    /* Output:
        Результат выполнения начальной задачи: 27
        Выполняется метод Main...
    */
}

private static TResult op<T1, T2, TResult>(
    Func<T1, T2, TResult> f, T1 arg1, T2 arg2) => f(arg1, arg2);

private static void display<T>(T res) =>
    Console.WriteLine($"Результат выполнения начальной задачи: {res}");
```
____

## Класс Parallel

**TPL** также включает в себя класс **```Parallel```**, который предназначен для упрощения параллельного
выполнения кода.

***1.*** Метод **```Invoke```** служит для параллельного выполнения задач и в качестве параметра
принимает массив объектов **```Action```**, таким образом, в метод можно передавать набор методов,
которые будут вызываться при его выполнении:

```csharp
public static void Main(string[] args)
{
    Parallel.Invoke(
  /*1*/ display,
  /*2*/ () => factorial(10),
  /*3*/ () => {
            Console.WriteLine($"Задача {Task.CurrentId} выполняется");
            Thread.Sleep(3000);
            Console.WriteLine($"Задача {Task.CurrentId} выполнена");
        });

    Console.WriteLine("Выполняется метод Main...");

    /* Output:
        Задача 3 выполняется
        Задача 1 выполняется
        Задача 2 выполняется
        Задача 3 выполнена
        Задача 1 выполнена (факториал числа 10 равен: 3628800)
        Задача 2 выполнена
        Выполняется метод Main...
    */
}

private static void display()
{
    Console.WriteLine($"Задача {Task.CurrentId} выполняется");
    Thread.Sleep(3000);
    Console.WriteLine($"Задача {Task.CurrentId} выполнена");
}

private static void factorial(int x)
{
    Console.WriteLine($"Задача {Task.CurrentId} выполняется");
    Thread.Sleep(3000);

    var n = (byte)x;
    ulong factorial = 1;

    while (n > 0) factorial *= n--;

    Console.WriteLine($"Задача {Task.CurrentId} выполнена" +
        $" (факториал числа {x} равен: {factorial})");
}
```

***2.*** Метод **```For```** помогает выполнять итерации цикла параллельно. В качестве значений параметров
методу необходимо передать начальный индекс элемента в цикле, конечный индекс и делегат
**```Action<int>```**, указывающий на метод, который будет выполняться один раз за итерацию:

> Итерируемый метод в качестве параметра должен принимать значение типа **```int```** – представляющее
счётчик, который проходит в цикле от 1...10 включительно, таким образом, метод вызывается
10 раз.

```csharp
public static void Main(string[] args)
{
    Parallel.For(1, 11, factorial);

    /* Output:
        Задача 5 выполняется...
        Задача 1 выполняется...
        Задача 2 выполняется...
        Задача 4 выполняется...
        Задача 3 выполняется...
        Задача 6 выполняется...
        Задача 7 выполняется...
        Задача 8 выполняется...
        Факториал числа 9 равен: 362880
        Факториал числа 7 равен: 5040
        Факториал числа 5 равен: 120
        Факториал числа 3 равен: 6
        Факториал числа 1 равен: 1
        Задача 1 выполняется...
        Задача 10 выполняется...
        Факториал числа 2 равен: 2
        Факториал числа 4 равен: 24
        Факториал числа 6 равен: 720
        Факториал числа 8 равен: 40320
        Факториал числа 10 равен: 3628800
    */
}

private static void factorial(int x)
{
    Console.WriteLine($"Задача {Task.CurrentId} выполняется...");
    Thread.Sleep(3000);

    var n = (byte)x;
    ulong factorial = 1;

    while (n > 0) factorial *= n--;

    Console.WriteLine($"Факториал числа {x} равен: {factorial}");
}
```

***3.*** Метод **```ForEach```** осуществляет параллельное выполнение перебора коллекции, реализующей
интерфейс **```IEnumerable```**. В качестве значений параметров методу нужно передавать перебираемую
коллекцию и делегат, выполняющийся один раз за итерацию для каждого перебираемого элемента
коллекции:

> Метод возвращает структуру **```ParallelLoopResult```**, которая содержит информацию о выполнении
цикла.

```csharp
public static void Main(string[] args)
{
    var result = Parallel.ForEach<int>(
        new List<int>() { 7, 10, 3 },
        factorial);

    /* Output:
        Задача 2 выполняется...
        Задача 1 выполняется...
        Задача 3 выполняется...
        Факториал числа 10 равен: 3628800
        Факториал числа 7 равен: 5040
        Факториал числа 3 равен: 6
    */
}

private static void factorial(int x)
{
    Console.WriteLine($"Задача {Task.CurrentId} выполняется...");
    Thread.Sleep(3000);

    var n = (byte)x;
    ulong factorial = 1;

    while (n > 0) factorial *= n--;

    Console.WriteLine($"Факториал числа {x} равен: {factorial}");
}
```

***4.*** Методы **```For```** и **```ForEach```** разрешают преждевременный выход из цикла с помощью вспомогательного
метода **```Break```**.
Для этого в итерируемый метод передают дополнительный параметр – объект **```ParallelLoopState```**,
метод **```Break```** которого, вызывают при достижении определённого условия, таким образом система
осуществит выход и прекратит выполнение метода **```For```** при первом удобном случае.

```csharp
public static void Main(string[] args)
{
    var result = Parallel.For(1, 11, factorial);
    if (!result.IsCompleted)
        Console.WriteLine("Выполнение цикла завершено на итерации: {0}",
            result.LowestBreakIteration);

    /* Output:
        Задача 1 выполняется...
        Факториал числа 1 равен: 1
        Задача 1 выполняется...
        Факториал числа 2 равен: 2
        Задача 2 выполняется...
        Факториал числа 3 равен: 6
        Выполнение цикла завершено на итерации: 3
    */
}

private static void factorial(int x, ParallelLoopState pls)
{
    var n = (byte)x;
    ulong factorial = 1;

    while (n > 0) {
        if (n == 3) pls.Break();
        factorial *= n--;
    }
    Console.WriteLine($"Задача {Task.CurrentId} выполняется...");
    Console.WriteLine($"Факториал числа {x} равен: {factorial}");
}
```

