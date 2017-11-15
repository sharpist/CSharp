using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var action = new Action();
            // подписка методов
            action.MyDelegate = method;
            action.MyDelegate = (() => method(0));
            /*
            во втором случае, используется лямбда-выражение для "обёртки" метода,
            не соответствующего сигнатуре, ожидаемой делегатом

            метод с параметром вызывается его оригиналом с верной сигнатурой (без параметра)
            private static void method()
            { method(param); }
            */

            action.MyDelegate();
        }

        // группа методов
        private static void method()
        { Console.WriteLine("без параметра"); }
        private static void method(int param)
        { Console.WriteLine("c параметром"); }
    }

    class Action
    {
        // делегат
        public delegate void typeDelegate(); /// сигнатура метода без параметра!
        private typeDelegate myDelegate;

        // доступ к делегату
        public typeDelegate MyDelegate
        { get { return this.myDelegate; } set { this.myDelegate += value; } }
    }
}
