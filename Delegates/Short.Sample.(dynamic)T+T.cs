using System;

namespace Example
{
    class Program
    {
        // делегат Func ссылается на метод MetodSumming,
        // в который передаются аргументы arg1 и arg2
        static T Calc<T>(Func<T, T, T> f, T arg1, T arg2)
            => f(arg1, arg2);



        // метод MetodSumming на который указывает делегат
        // возвращает сумму значений
        static T MetodSumming<T>(T param1, T param2)
            => (dynamic)param1 + param2;



        // главный метод Main точка входа
        // тестируем class Program
        static void Main()
        {
            Console.WriteLine(Calc(MetodSumming, 12, 13));               // 25

            Console.WriteLine(Calc(MetodSumming, 2.9, 1.1));             // 4

            Console.WriteLine(Calc(MetodSumming, "C", "#"));             // C#

            // используется собственный тип объекта
            Console.WriteLine(Calc(MetodSumming, new ObjectType(6),
                                                 new ObjectType(4)));    // 10

            Console.WriteLine(Calc(MetodSumming, new ObjectType(6), 4)); // 10

            Console.WriteLine(Calc(MetodSumming, 6, new ObjectType(4))); // 10
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
