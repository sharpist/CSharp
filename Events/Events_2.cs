using System;

/// <summary>
/// Порядок создания события:
/// 1. определить условие возникновения события и методы, которые должны сработать
/// 2. определить сигнатуру (прототип) методов и создать делегат на основе сигнатуры
/// 3. создать общедоступное событие на основе делегата и вызвать, когда условие сработает
/// 4. подписаться (где-угодно) на событие теми методами, которые должны сработать и сигнатуры которых соответствуют делегату
/// </summary>
public class Program
{
    public static void Main()
    {
        var pblshr = new EventPublisher();
        var sbscr1 = new EventSubscriber_I();
        var sbscr2 = new EventSubscriber_II();

        pblshr.onCount += sbscr1.Message; // подписка на событие
        pblshr.onCount += sbscr2.Message;

        pblshr.Count(); // запуск счетчика
    }

    // прямо здесь в программе может быть определен обработчик события, который реализует любую! логику
}

// класс данных события
class EventArgs
{
    public string Message { get; }
    public EventArgs(string mes) => Message = mes;
}

// класс-издатель создает событие (генерирует)
class EventPublisher
{
    // public delegate <выходной тип> НазваниеДелегата(<тип входных параметров>);
    public delegate void MethodContainer(object sender, EventArgs e);
    private event MethodContainer _onCount;
    // public event <НазваниеДелегата> <НазваниеСобытия>;
    public event MethodContainer onCount
    {
        add {
            _onCount += value;
            Console.WriteLine($"{value.Method.Name} добавлен");
        }
        remove {
            _onCount -= value;
            Console.WriteLine($"{value.Method.Name} удален");
        }
    }

    public void Count()
    {
        for (int i = 0; i < 10; i++)

            if (i == 7) { // условие возникновения события
                _onCount?.Invoke(this, new EventArgs($"Вызвано событие т.к. счетчик равен {i}"));
                return;
            }
    }
}

// классы-подписчики реагируют на событие
class EventSubscriber_I
{
    public void Message(object sender, EventArgs e) => // обработчик события
        Console.WriteLine(e.Message);
}

class EventSubscriber_II
{
    public void Message(object sender, EventArgs e) =>
        Console.WriteLine(e.Message);
}
