using System.Diagnostics;
using static System.Console;

class Program
{
    static void Main()
    {
        uint size = 80000000;
        var A = new int[size];
        #region "B" array filling
        var B = new int[size];
        for (int i = 0; i < size; i++)
            B[i] = i + 1;
        #endregion


        var watch = Stopwatch.StartNew();
        NormalLoop(size, A, B);
        watch.Stop();
        // Normal loop
        WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds); // Elapsed ms: ~614

        watch = Stopwatch.StartNew();
        AfterLoopUnrolling(size, A, B);
        watch.Stop();
        // After loop unrolling
        WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds); // Elapsed ms: ~320

        watch = Stopwatch.StartNew();
        DuffsDevice(size, A, B);
        watch.Stop();
        // Duff's device a functionally equivalent version
        WriteLine("Elapsed ms: " + watch.ElapsedMilliseconds); // Elapsed ms: ~307
    }

    static void NormalLoop(uint count, int[] A, int[] B)
    {
        for (int i = 0; i < count; i++)
        {
            A[i] = B[i];
        }
    }

    // ***this optimization consisting in an artificial
    // increase in the number of instructions
    static void AfterLoopUnrolling(uint count, int[] A, int[] B)
    {
        for (int i = 0; i < count - 4; i += 5)
        {
            A[i] = B[i];
            A[i + 1] = B[i + 1];
            A[i + 2] = B[i + 2];
            A[i + 3] = B[i + 3];
            A[i + 4] = B[i + 4];
        }
    }

    // ***Duff's device is a way of manually implementing loop unrolling by
    // interleaving two syntactic constructs the while loop and a switch statement
    static void DuffsDevice(uint count, int[] A, int[] B)
    {
        uint i = 0;
        uint n = (count + 7) / 8;
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
            A[i] = B[i];
            A[i + 1] = B[i + 1];
            A[i + 2] = B[i + 2];
            A[i + 3] = B[i + 3];
            A[i + 4] = B[i + 4];
            A[i + 5] = B[i + 5];
            A[i + 6] = B[i + 6];
            A[i + 7] = B[i + 7];
            i += 8;
        }
    }
}
