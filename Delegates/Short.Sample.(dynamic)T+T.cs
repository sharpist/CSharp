using System;

public class Program
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
    static void Main()
    {
        int result = Calc(MetodSumming, 12, 13);

        Console.WriteLine(result); // 25
    }
}

