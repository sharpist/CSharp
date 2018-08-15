
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


    public void Recursion(int i = 0)
    {
        if (i++ == 5000)          // exit
            return;

        fibonacci(new int[2000]); // any custom operations

        Recursion(i);             // recursive call


        // local Fibonacci procedure
        void fibonacci(int[] fib, int j = 0)
        {
            fib[j + 2] = (j != 0) ?
                fib[j + 1] + fib[j] : (fib[j + 1] = fib[j] = 1) + fib[j];

            if (++j == fib.Length - 2) // exit
                return;

            fibonacci(fib, j);         // recursive call
        }
    }
}
