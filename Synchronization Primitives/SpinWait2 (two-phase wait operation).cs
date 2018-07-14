#define LOGGING

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using static System.Threading.Interlocked;

class Latch
{
    #region LOGGING
#if LOGGING
    // для быстрого логирования с минимальным влиянием на поведение блокировки
    // количество вращений больше 20 может зависеть от конфигурации компьютера
    private long[] spinCountLog = new long[20];

    public void DisplayLog()
    {
        for (int i = 0; i < spinCountLog.Length; i++)
        {
            WriteLine("Wait succeeded with spin count of {0} on {1:N0} attempts",
                              i, spinCountLog[i]);
        }
        WriteLine("Wait used the kernel event on {0:N0} attempts.", totalKernelWaits);
        WriteLine("Logging complete");
    }
#endif
    #endregion
    // 0 = unset, 1 = set.
    private int m_state = 0;
    private volatile int totalKernelWaits = 0;
    // блок потоков, ожидающих событие ManualResetEvent
    private ManualResetEvent m_ev = new ManualResetEvent(false);
    // lock
    private object latchLock = new object();

    public void Set()
    {
        lock (latchLock) {
            m_state = 1;
            m_ev.Set(); // уведомление о событии (ManualResetEvent = true)
        }
    }

    public bool Wait(int timeout)
    {
        var spinner = new SpinWait();

        while (m_state == 0)
        {
            // запустить секундомер для отслеживания тайм-аута
            var watch = Stopwatch.StartNew();

            // прокрутить только пока SpinWait не готов
            // инициировать переключение контекста
            if (!spinner.NextSpinWillYield) spinner.SpinOnce();
            // вместо того, чтобы позволить SpinWait выполнить сейчас
            // переключение контекста, запускаем операцию ожидания
            // ядра – это запланировано сделать в любом случае
            else
            {
                Increment(ref totalKernelWaits);
                // учесть затраченное время
                long realTimeout = timeout - watch.ElapsedMilliseconds;
                // ожидание
                if (realTimeout <= 0 || !m_ev.WaitOne((int)realTimeout))
                {
                    Trace.WriteLine("wait timed out.");
                    return false;
                }
            }
        }

#if LOGGING
        Increment(ref spinCountLog[spinner.Count]);
#endif
        // взять блокировку
        Exchange(ref m_state, 0);
        return true;
    }
    public void Wait()
    {
        Trace.WriteLine("Wait timeout infinite");
        Wait(Timeout.Infinite);
    }
}

class Example
{
    static int count = 2;
    static Latch latch = new Latch();
    static CancellationTokenSource cts = new CancellationTokenSource();

    static void TestMethod()
    {
        // выполнять до запроса отмены
        while (!cts.IsCancellationRequested)
        {
            // получить блокировку
            if (latch.Wait(50))
            {
                // делать работу
                // здесь варьируется рабочая нагрузка, чтобы вызвать
                // изменение количества вращений в кратковременной блокировке
                double d = 0;
                if (count % 2 != 0) d = Math.Sqrt(count);

                Increment(ref count);

                // сбросить блокировку
                latch.Set();
            }
        }
    }

    static void Main()
    {
        /// <summary>
        /// Демонстрация кратковременной блокировки:
        /// 
        /// несколько потоков, обновляющих общее целое число
        /// операции выполняются относительно быстро, что позволяет
        /// кратковременной блокировке демонстрировать успешные
        /// ожидания только путем вращения
        /// </summary>
        latch.Set();

        // поток пользовательского интерфейса запускает задачу-
        // -запрос "нажмите 'C' для отмены цикла"
        Task.Factory.StartNew(() =>
        {
            WriteLine("Press 'c' to cancel.");
            if (ReadKey(true).KeyChar == 'c')
                cts.Cancel();
        });
        // в этот момент параллельно запускаются задания
        Parallel.Invoke(() => TestMethod(),
                        () => TestMethod(),
                        () => TestMethod());

#if LOGGING
        latch.DisplayLog();
        if (cts != null) cts.Dispose();
#endif
    }
    /*
    Press 'c' to cancel.
    Wait succeeded with spin count of 0 on 79 349 attempts
    Wait succeeded with spin count of 1 on 368 043 attempts
    Wait succeeded with spin count of 2 on 52 149 attempts
    Wait succeeded with spin count of 3 on 208 278 attempts
    Wait succeeded with spin count of 4 on 126 928 attempts
    Wait succeeded with spin count of 5 on 6 818 attempts
    Wait succeeded with spin count of 6 on 564 attempts
    ...
    Wait succeeded with spin count of 19 on 0 attempts
    Wait used the kernel event on 6 449 attempts.
    Logging complete
    */
}