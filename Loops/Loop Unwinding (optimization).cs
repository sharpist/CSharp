using System.Diagnostics;
using static System.Console;

class Program
{
    static Stopwatch watch;
    static int[] A = new int[70000000],
                 B = new int[70000000];

    static void Main()
    {
        #region "B" array filling
        for (int i = 0; i < 70000000; i++)
                 B[i] = i + 1;
        #endregion


        watch = Stopwatch.StartNew();
        NormalLoop();
        watch.Stop();
        // Normal loop
        WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds); // Elapsed ms: ~1680

        watch = Stopwatch.StartNew();
        AfterLoopUnrolling();
        watch.Stop();
        // After loop unrolling
        WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds); // Elapsed ms: ~1400

        watch = Stopwatch.StartNew();
        DuffsDevice();
        watch.Stop();
        // Duff's device a functionally equivalent version
        WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds); // Elapsed ms: ~380 (not used "Mod" operation)
    }

    static void NormalLoop()
    {
        for (int i = 0; i < 70000000; i++)
        {
            A[i] = (i % B[i]);
        }
    }

    // ***this optimization consisting in an artificial
    // increase in the number of instructions
    static void AfterLoopUnrolling()
    {
        for (int i = 0; i < 70000000 - 4; i += 5)
        {
            A[i] = (i % B[i]);
            A[i + 1] = ((i + 1) % B[i + 1]);
            A[i + 2] = ((i + 2) % B[i + 2]);
            A[i + 3] = ((i + 3) % B[i + 3]);
            A[i + 4] = ((i + 4) % B[i + 4]);
        }
    }

    // ***Duff's device is a way of manually implementing loop unrolling by
    // interleaving two syntactic constructs the while loop and a switch statement
    static void DuffsDevice()
    {
        int i = 0;
        int count = 70000000;

        int n = (count + 7) / 8;
        switch (count % 8) {
            case 0: A[i] = B[i++]; goto case 7;
            case 7: A[i] = B[i++]; goto case 6;
            case 6: A[i] = B[i++]; goto case 5;
            case 5: A[i] = B[i++]; goto case 4;
            case 4: A[i] = B[i++]; goto case 3;
            case 3: A[i] = B[i++]; goto case 2;
            case 2: A[i] = B[i++]; goto case 1;
            case 1: A[i] = B[i++]; break;
        }
        while (--n > 0) {
            A[i] = B[i++];
            A[i] = B[i++];
            A[i] = B[i++];
            A[i] = B[i++];
            A[i] = B[i++];
            A[i] = B[i++];
            A[i] = B[i++];
            A[i] = B[i++];
        }
    }
}
