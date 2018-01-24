using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using static System.Console;

class Program
{
    static void Main()
    {
        var sc    = new SynchronizedCache();
        var tasks = new List<Task>();
        int itemsWritten = 0;


        // записывающая задача
        tasks.Add(Task.Run(() => {
            String[] vegetables = { "брокколи",             "цветная капуста",    "морковь",
                                    "щавель",               "репа",               "свекла",
                                    "брюссельская капуста", "капуста",            "лук",
                                    "шпинат",               "виноградные листья", "петрушка",
                                    "кукуруза",             "томат",              "огурец",
                                    "сельдерей",            "радиккио",           "лимская фасоль",
                                    "редис",                "чеснок",             "листья лайма" };

            for (int ctr = 1; ctr <= vegetables.Length; ctr++) // заполнение кэша
                sc.Add(ctr, vegetables[ctr - 1]);

            itemsWritten = vegetables.Length;
            WriteLine($"Задача {Task.CurrentId} записала {itemsWritten} пункт\n");
        }));


        // 2 читающие задачи (readers)
        // первая читает от начала, вторая - с конца
        for (int ctr = 0; ctr <= 1; ctr++)
        {
            bool desc = Convert.ToBoolean(ctr);
            tasks.Add(Task.Run(() => { Thread.Sleep(250);

                int start, last, step;
                int items = sc.Count;
                do
                {
                    // инициализация start, last, step
                    if (!desc) { start = 1;     last = items; step = 1; }
                    else       { start = items; last = 1;     step = -1; }


                    String output = String.Empty;
                    for (int index = start; desc ? index >= last : index <= last; index += step)
                        output += String.Format($"[{sc.Read(index)}] ");

                    WriteLine($"Задача {Task.CurrentId} прочитала {items} пункт: {output}\n");

                } while (items < itemsWritten | itemsWritten == 0);
            }));
        }


        // читающая/обновляющая задача
        tasks.Add(Task.Run(() => { Thread.Sleep(500);

            for (int ctr = 1; ctr <= sc.Count; ctr++)
            {
                String value = sc.Read(ctr);
                if (value == "огурец" &&
                (sc.AddOrUpdate(ctr, "картофель") != SynchronizedCache.AddOrUpdateStatus.Unchanged))
                    WriteLine("Изменён 'огурец' на 'картофель'");
            }
        }));


        // дождаться завершения всех 4 задач
        Task.WaitAll(tasks.ToArray());
        // отобразить результирующее содержимое кэша
        WriteLine("Значения в синхронизированном кэше: ");
        for (int ctr = 1; ctr <= sc.Count; ctr++) Write($" {ctr}:\t{sc.Read(ctr)}\n");
    }
}




class SynchronizedCache
{
    private ReaderWriterLockSlim    rwls  = new ReaderWriterLockSlim();    // синхронизатор
    private Dictionary<int, string> cache = new Dictionary<int, string>(); // кэш

    public enum AddOrUpdateStatus { Added, Updated, Unchanged }

    public int Count => cache.Count;


    public string Read(int key)
    {
        rwls.EnterReadLock();
        try
        { return cache[key]; }

        finally
        { rwls.ExitReadLock(); }
    }

    public void Add(int key, string value)
    {
        rwls.EnterWriteLock();
        try
        { cache.Add(key, value); }

        finally
        { rwls.ExitWriteLock(); }
    }

    public bool AddWithTimeout(int key, string value, int timeout)
    {
        if (rwls.TryEnterWriteLock(timeout))
        {
            try
            { cache.Add(key, value); }

            finally
            { rwls.ExitWriteLock(); }
            return true;
        }
        else return false;
    }

    public AddOrUpdateStatus AddOrUpdate(int key, string value)
    {
        rwls.EnterUpgradeableReadLock(); // обновляемая блокировка чтения
        try
        {
            string result = null;
            if (cache.TryGetValue(key, out result))
            {
                if (result == value) return AddOrUpdateStatus.Unchanged;
                else
                {
                    rwls.EnterWriteLock(); // переход в блокировку записи без снятия блокировки чтения
                    try
                    { cache[key] = value; }

                    finally
                    { rwls.ExitWriteLock(); } // выход из режима записи
                    return AddOrUpdateStatus.Updated;
                }
            }
            else
            {
                rwls.EnterWriteLock();
                try
                { cache.Add(key, value); }

                finally
                { rwls.ExitWriteLock(); }
                return AddOrUpdateStatus.Added;
            }
        }

        finally
        { rwls.ExitUpgradeableReadLock(); } // выход из обновляемого режима
    }

    public void Delete(int key)
    {
        rwls.EnterWriteLock();
        try
        { cache.Remove(key); }

        finally
        { rwls.ExitWriteLock(); }
    }


    ~SynchronizedCache() { // деструктор
        if (rwls != null) rwls.Dispose();
    }
}

/*
Задача 1 записала 21 пункт

Задача 2 прочитала 21 пункт: [листья лайма] [чеснок] [редис] [лимская фасоль] [радиккио] [сельдерей] [огурец] [томат] [кукуруза] [петрушка] [виноградные листья] [шпинат] [лук] [капуста] [брюссельская капуста] [свекла] [репа] [щавель] [морковь] [цветная капуста] [брокколи]

Задача 3 прочитала 21 пункт: [брокколи] [цветная капуста] [морковь] [щавель] [репа] [свекла] [брюссельская капуста] [капуста] [лук] [шпинат] [виноградные листья] [петрушка] [кукуруза] [томат] [огурец] [сельдерей] [радиккио] [лимская фасоль] [редис] [чеснок] [листья лайма]

Изменён 'огурец' на 'картофель'
Значения в синхронизированном кэше:
 1:     брокколи
 2:     цветная капуста
 3:     морковь
 4:     щавель
 5:     репа
 6:     свекла
 7:     брюссельская капуста
 8:     капуста
 9:     лук
 10:    шпинат
 11:    виноградные листья
 12:    петрушка
 13:    кукуруза
 14:    томат
 15:    картофель
 16:    сельдерей
 17:    радиккио
 18:    лимская фасоль
 19:    редис
 20:    чеснок
 21:    листья лайма
 */
