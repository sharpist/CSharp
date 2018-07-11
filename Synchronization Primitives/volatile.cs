using System.Threading;
using static System.Console;

/// <summary>
/// Слово volatile означает, что поле может изменить несколько потоков,
/// выполняемых одновременно.
/// 
/// Поля, объявленные volatile, не участвуют в оптимизации компилятора,
/// предполагающей доступ для одного потока. Это гарантирует, что в любой
/// момент времени в поле будет содержаться актуальное значение.
/// 
/// Обычно модификатор volatile используется для поля, к которому
/// обращаются несколько потоков.
/// Можно применять только к полям класса или структуры.
/// 
/// Пример демонстрирует создание вспомогательного или рабочего потока
/// и его применение для выполнения обработки параллельно с основным потоком.
/// </summary>
class Worker
{
    // этот метод вызывается при запуске потока
    public void DoWork()
    {
        while (!_shouldStop)
            WriteLine("Worker thread: working...");

        WriteLine("Worker thread: terminating gracefully.");
    }

    public void RequestStop() =>
        _shouldStop = true;

    // слово volatile используется как подсказка компилятору,
    // что к этому члену данных обращаются несколько потоков
    private volatile bool _shouldStop;
}

class WorkerThreadExample
{
    static void Main()
    {
        // создать объект рабочего потока (это не запускает поток)
        var workerObject = new Worker();
        var workerThread = new Thread(workerObject.DoWork);
        // запустить рабочий поток
        workerThread.Start();
        WriteLine("Main thread: starting worker thread...");


        // петля до тех пор, пока рабочий поток не активируется
        while (!workerThread.IsAlive) ;
        // перевести основной поток в спящий режим на 1 миллисекунду,
        // чтобы рабочий поток мог выполнить некоторую работу
        Thread.Sleep(1);


        // запросить остановку рабочего потока
        workerObject.RequestStop();
        // метод Thread.Join блокирует вызывающий поток до завершения
        workerThread.Join();
        WriteLine("Main thread: worker thread has terminated.");
    }
    /*
    Main thread: starting worker thread...
    Worker thread: working...
    Worker thread: working...
    Worker thread: working...
    ...
    Worker thread: working...
    Worker thread: working...
    Worker thread: working...
    Worker thread: terminating gracefully.
    Main thread: worker thread has terminated.
    */
}