using System;

class Program
{
    static unsafe void Copy(byte[] src, int srcIndex,
                            byte[] dst, int dstIndex, uint count)
    {
        fixed (byte* pSrc = src, pDst = dst)
        {
            byte* ps = pSrc,
                  pd = pDst;
            // копируются блоки по 4 байта
            // за проход копируется целое число int
            for (uint i = count / 4; i > 0; i--)
            {
                *((int*)pd) = *((int*)ps);
                ps += 4;
                pd += 4;
            }
            // копируются оставшиеся байты
            switch (count % 4)
            {
                case 3: *pd++ = *ps++; goto case 2;
                case 2: *pd++ = *ps++; goto case 1;
                case 1: *pd++ = *ps++; break;
                default: break;
            }
        }
    }
    static void Main()
    {
        byte[] src = new byte[100],
               dst = new byte[100];

        // инициализация src
        for (int i = 0; i < 100; i++)
            src[i] = (byte)i;

        // копирование src в dst
        Copy(src, 0, dst, 0, 100);

        Console.WriteLine("первые 10 элементов:");
        for ((int i, int ten) = (0, 10); i < ten; i++)
            Console.Write("{0}{1}", dst[i], (i + 1 < ten) ? " " : "\n");
        /*
            первые 10 элементов:
            0 1 2 3 4 5 6 7 8 9
        */
    }
}
