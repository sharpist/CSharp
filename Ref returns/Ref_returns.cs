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
        for (int ctr = 0; ctr < numbers.Length; ctr++)
            if (numbers[ctr] >= target)
                return ref numbers[ctr];

        return ref numbers[0];
    }

    public override string ToString() => string.Join(" ", numbers);
}
