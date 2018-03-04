using System;
using System.Drawing;
using System.Threading;

namespace Moving_Objects
{
    public class SynchronizedCache
    {
        private ReaderWriterLockSlim rwls  = new ReaderWriterLockSlim(); // синхронизатор
        private PointF[] cache; // защищаемый кэш

        // операции с кэшем
        public int Count => cache.Length;
        public PointF Read(int index)
        {
            rwls.EnterReadLock();
            try { return cache[index]; }
            finally { rwls.ExitReadLock(); }
        }
        public void Add(int index, Tuple<short, short> vectors)
        {
            rwls.EnterWriteLock();
            try { cache[index].X += vectors.Item1; cache[index].Y += vectors.Item2; }
            finally { rwls.ExitWriteLock(); }
        }

        public SynchronizedCache(int size) {
            cache = new PointF[size];
        }
        ~SynchronizedCache() { // деструктор
            if (rwls != null) rwls.Dispose();
        }
    }
}
