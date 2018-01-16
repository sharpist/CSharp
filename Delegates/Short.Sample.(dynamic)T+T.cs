using System;

namespace Example
{
    class Program
    {
        // делегат Func ссылается на метод MetodSumming,
        // в который передаются аргументы arg1 и arg2
        static TResult Calc<T1, T2, TResult>(Func<T1, T2, TResult> f, T1 arg1, T2 arg2)
            => f(arg1, arg2);



        // метод MetodSumming на который указывает делегат
        // возвращает сумму значений
        static TResult MetodSumming<T1, T2, TResult>(T1 param1, T2 param2)
            => (dynamic)param1 + param2;



        // главный метод Main точка входа
        // тестируем class Program
        static void Main()
        {
            Console.WriteLine(Calc(MetodSumming<int, int, int>,                  12,  13)); // 25

            Console.WriteLine(Calc(MetodSumming<double, double, double>,         2.9, 1.1)); // 4

            Console.WriteLine(Calc(MetodSumming<string, char, string>,           "C", '#')); // C#

            // используется собственный тип объекта
            Console.WriteLine(Calc(MetodSumming<ObjectType, ObjectType, object>, new ObjectType(6), new ObjectType(4))); // 10

            Console.WriteLine(Calc(MetodSumming<ObjectType, int, object>,        new ObjectType(6), 4)); // 10

            Console.WriteLine(Calc(MetodSumming<int, ObjectType, object>,        6, new ObjectType(4))); // 10
        }
    }



    class ObjectType
    {
        private int value { get; set; }
        public ObjectType(int param) {
            this.value = param;
        }

        public override string ToString() => $"{this.value}";



        // оператор неявного преобразования
        public static implicit operator ObjectType(int from)
            => new ObjectType(from);
        // оператор явного преобразования
        public static explicit operator int(ObjectType from)
            => from.value;



        // перегружаемый оператор +
        public static ObjectType operator +(ObjectType lhs, ObjectType rhs)
            => new ObjectType(lhs.value + rhs.value);
        // перегружаемый оператор -
        public static ObjectType operator -(ObjectType lhs, ObjectType rhs)
            => new ObjectType(lhs.value - rhs.value);
    }
}