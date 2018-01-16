using System;

class Program
{
    public static TResult Calc<T1, T2, TResult>
        (Func<T1, T2, TResult> f,
                                  T1 a, T2 b) => f(a, b);



    static void Main()
    {
        // используется лямбда выражение
        var result1 = Calc((a, b) => a += b,         4, 3); // 7
        var result2 = Calc((a, b) => a += b,     4.9, 5.1); // 10
        var result3 = Calc((a, b) => a += b, "C", "Sharp"); // CSharp

        Console.WriteLine($"{result1}\n{result2}\n{result3}");



        // используется анонимный метод
        result1 = Calc(delegate (int a, int b)       { return a += b; },         4, 3); // 7
        result2 = Calc(delegate (double a, double b) { return a += b; },     4.9, 5.1); // 10
        result3 = Calc(delegate (string a, string b) { return a += b; }, "C", "Sharp"); // CSharp

        Console.WriteLine($"{result1}\n{result2}\n{result3}");
    }
}