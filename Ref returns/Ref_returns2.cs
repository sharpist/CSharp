using static System.Console;

class Program
{
    static void Main()
    {
        var store = new NumberStore();
        WriteLine($"Исходная последовательность: {store.ToString()}");

        ref var value = ref store.FindNumber(16);
        value *= 2;
        WriteLine($"Новая последовательность:    {store.ToString()}");
        // Исходная последовательность: 1 3 7 15 31 63 127 255 511 1023
        // Новая последовательность:    1 3 7 15 62 63 127 255 511 1023
    }
}
class NumberStore
{
    int[] numbers = { 1, 3, 7, 15, 31, 63, 127, 255, 511, 1023 };

    public ref int FindNumber(int target)
    {
        ref int returnVal = ref numbers[0];
        var ctr = numbers.Length - 1;
        while ((ctr > 0) && numbers[ctr] >= target)
        {
            returnVal = ref numbers[ctr];
            ctr--;
        }
        return ref returnVal;
    }

    public override string ToString() => string.Join(" ", numbers);
}
