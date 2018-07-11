using System.Threading;
using static System.Console;
using static System.Threading.Interlocked;

/// <summary>
/// SpinWait – упрощенный тип синхронизации, который позволяет обойтись
/// в низкоуровневых сценариях без ресурсоемких переключений контекста и
/// изменений режима ядра, которые выполняются для событий ядра.
/// 
/// На многоядерных компьютерах, если сценарий не подразумевает долгосрочную
/// блокировку ресурсов, эффективнее всего применить цикл ожидания в
/// пользовательском режиме для потока в состоянии ожидания, который после
/// нескольких десятков или сотен повторений снова попытается обратиться к
/// ресурсу.
/// 
/// Если ресурс окажется доступен после выхода из такого цикла, экономится
/// несколько тысяч тактов процессора.
/// Если же ресурс будет недоступен, то потрачено лишь несколько циклов и
/// можно снова применить ожидание на уровне ядра.
/// 
/// Такой подход с циклом и последующим ожиданием иногда называют
/// "двухэтапной операцией ожидания".
/// </summary>

class LockFreeStack<T>
{
    public class Node { public Node Next; public T Value; }
    // к этому члену данных обращаются несколько потоков
    private volatile Node m_head;
    public Node Head => this.m_head;
 


    public void Push(T item)
    {
        Node head;
        Node node = new Node { Value = item };
        var spin  = new SpinWait();

        while (true)
        {
            head = m_head;
            node.Next = head;
            // при равенстве m_head и head, node копируется в m_head,
            // если изначальное значение m_head равно head выйти из цикла
            if (CompareExchange(ref m_head, node, head) == head) break;

            // выполняет одну прокрутку
            if (spin.NextSpinWillYield == true) spin.SpinOnce();
        }
    }

    public bool TryPop(out T result)
    {
        Node head;
        result   = default(T);
        var spin = new SpinWait();

        while (true)
        {
            head = m_head;
            if (head == null) return false;
            if (CompareExchange(ref m_head, head.Next, head) == head)
            {
                result = head.Value;
                return true;
            }
            if (spin.NextSpinWillYield == true) spin.SpinOnce();
        }
    }
}

class Program
{
    static void Main()
    {
        var lfs = new LockFreeStack<int>();
        // создать 50 потоков, которые вызывают
        // метод Push на одном!!! и том же экземпляре LockFreeStack
        var threads = new Thread[50];
        for ((int i, int j) = (0, 1); i < 50; i++)
            threads[i] = new Thread(new ThreadStart(() => lfs.Push(j++)));

        // запуск всех потоков
        foreach (Thread t in threads)
            t.Start();

        Thread.Sleep(150); // ждать 150 мс


        // вывести записанные значения
        var nodes = lfs.Head;
        Write($"LockFreeStack contains these values:");
        while (nodes != null)
        {
            Write((nodes.Value%10==0?"\n":"  ")
                + $"{nodes.Value}");

            nodes = nodes.Next;
        }
    }
    // LockFreeStack contains these values:
    // 50  49  48  47  46  45  44  43  42  41
    // 40  39  38  37  36  35  34  33  32  31
    // 30  29  28  27  26  25  24  23  22  21
    // 20  19  18  17  16  15  14  13  12  11
    // 10  9  8  7  6  5  4  3  2  1
}