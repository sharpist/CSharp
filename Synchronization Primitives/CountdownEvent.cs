using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

class Example
{
    static void Main()
    {
        // инициализировать очередь и CountdownEvent
        var queue = new ConcurrentQueue<int>(Enumerable.Range(0, 10000));
        var cde   = new CountdownEvent(10000); // начальный счётчик = 10000


        // логика для всех пользователей очереди
        Action consumer = () => { int local;
            // уменьшить счётчик CDE на каждый элемент извлекаемый из очереди
            while (queue.TryDequeue(out local)) cde.Signal(); };

        // запустить асинхронные задачи для освобождение очереди
        Task t1 = Task.Factory.StartNew(consumer); // делегат consumer указывает на анонимный метод
        Task t2 = Task.Factory.StartNew(consumer);


        // возврат когда счётчик cde достигнет 0
        cde.Wait();           // дождаться опустошения очереди, ожидая cde
        Task.WaitAll(t1, t2); // рекомедуется дождаться завершения задач, даже если их работа выполнена

        WriteLine($"Очередь освобождена!\nначальный счётчик:\t{cde.InitialCount}\nтекущий счётчик:\t{cde.CurrentCount}\nустановка события:\t{cde.IsSet}\n");






        cde.Reset(10);   // сбросить InitialCount/CurrentCount на указанное значение
        cde.AddCount(2); // AddCount повлияет на CurrentCount, но не на InitialCount

        WriteLine($"После Reset(10), AddCount(2)\nначальный счётчик:\t{cde.InitialCount}\nтекущий счётчик:\t{cde.CurrentCount}\nустановка события:\t{cde.IsSet}\n");

        // пробовать ожидание с отменой
        var cts = new CancellationTokenSource();
        cts.Cancel(); // инициировать отмену задачи

        try
        {
            cde.Wait(cts.Token); // создать/передать предустановленный признак отмены
        }

        catch (OperationCanceledException) { WriteLine("cde.Wait(preCanceledToken) бросил OperationCanceledException при отмене"); }
        finally { cts.Dispose(); } cde.Dispose(); // высвобождение после использования
    }
}
/*
Очередь освобождена!
начальный счётчик:      10000
текущий счётчик:        0
установка события:      True

После Reset(10), AddCount(2)
начальный счётчик:      10
текущий счётчик:        12
установка события:      False

cde.Wait(preCanceledToken) бросил OperationCanceledException при отмене
*/
