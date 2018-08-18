class Program
{
    static void Main()
    {
        var prm = new Program();
        prm.Iteration();
        prm.Recursion();
    }


    public void Iteration()
    {
        for (int i = 0; i < 5000; i++)
        {
            // Fibonacci
            var fib = new int[2000];
            for (int j = 0; j != fib.Length - 2; j++)
            {
                fib[j + 2] = (j != 0) ?
                    fib[j + 1] + fib[j] : (fib[j + 1] = fib[j] = 1) + fib[j];
            }
        }
    }


    public void Recursion(ushort i = 0)
    {
        if (i++ == 8000)           // exit
            return;

        fibonacci(new uint[1000]); // any custom operations

        Recursion(i);              // recursive call


        // local Fibonacci procedure
        void fibonacci(uint[] fib, ushort j = 0)
        {
        START:

            fib[j + 2] = (j != 0) ?
                (fib[j + 1] + fib[j]) % 100000
                : (fib[j + 1] = fib[j] = 1) + fib[j];

            if (j++ == fib.Length - 3) // exit
                return;

        // recursive unrolling into iterations
        goto START;
        }
    }
}

