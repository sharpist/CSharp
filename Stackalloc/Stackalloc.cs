using static System.Console;

class Program
{
    static void Main()
    {
        unsafe
        {
            const int arraySize = 20;
            int* fib = stackalloc int[arraySize];
            int* p = fib;
            // начало последовательности с 1, 1
            *p++ = *p++ = 1;

            for (int i = 2; i < arraySize; ++i, ++p)
                // рассчитывается сумма предыдущих пар чисел
                *p = p[-1] + p[-2];


            for (int i = 0; i < arraySize; ++i) WriteLine(fib[i]);
            // 1
            // 1
            // 2
            // 3
            // 5
            // 8
            // 13
            // 21
            // 34
            // 55
            // 89
            // 144
            // 233
            // 377
            // 610
            // 987
            // 1597
            // 2584
            // 4181
            // 6765
        }
    }
}
