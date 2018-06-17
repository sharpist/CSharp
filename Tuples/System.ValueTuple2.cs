using static System.Console;

class Program
{
    static void Main()
    {
        var result = Method();

        WriteLine($"{result.Item1} {result.Item2} {result.Item3}");
        // Hello World !
    }

    static (string, string, char) Method()
    {
        // Note:
        // tuple syntax is involved to returns
        // a set of values of different types
        return ("Hello", "World", '!');
    }
}