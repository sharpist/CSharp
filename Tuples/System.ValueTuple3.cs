using static System.Console;

class Program
{ // переменные x и y меняются значениями
  // использован ValueTuple
    static void Main()
    {
        Swap();
    }

    static void Swap()
    {
        int x = 0, y = 42;

        (x, y) = (y, x);

        WriteLine($"now x: {x}, y: {y}");
        // now x: 42, y: 0
    }
}
