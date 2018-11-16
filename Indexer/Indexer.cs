using IndexersSamples.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IndexersSamples.SampleOne
{
    public class DataSamples
    {
        private readonly int totalSize;
        private readonly List<Page> pagesInMemory = new List<Page>();

        public DataSamples(int totalSize) => // конструктор DataSamples
            this.totalSize = totalSize;

        public Sample this[int index]        // индексатор DataSamples
        {
            get
            {
                if (index > 0 && index < totalSize)
                    return updateCachedPagesForAccess(index)[index];  // вызов индексатора Page
                else throw new IndexOutOfRangeException();
            }
            set
            {
                if (index > 0 && index < totalSize)
                    updateCachedPagesForAccess(index)[index] = value; // вызов индексатора Page
                else throw new IndexOutOfRangeException();
            }
        }

        private Page updateCachedPagesForAccess(int index) // обновить кэшированные страницы
        {
            foreach (var p in pagesInMemory)
                if (p.HasItem(index)) return p;

            var startingIndex = (index / 1000) * 1000;     // index    0 -  999 startingIndex    0
            var newPage = new Page(startingIndex, 1000);   // index 1000 - 1999 startingIndex 1000

            if (pagesInMemory.Count > 4)
            {
                var oldest = pagesInMemory // удалить самую старую чистую (холодную) страницу
                    .Where(page => !page.Dirty)
                    .OrderBy(page => page.LastAccess)
                    .FirstOrDefault();

                if (oldest != null) pagesInMemory.Remove(oldest); // может содержаться более 5 страниц в памяти
            }
            pagesInMemory.Add(newPage);    // добавить страницу в кэш
            return newPage;
        }

        private sealed class Page
        {
            public bool     Dirty      { get; private set; }
            public DateTime LastAccess { get; private set; }

            private readonly int startingIndex, length;
            private readonly List<Sample> pageData = new List<Sample>();

            public Page(int startingIndex, int length) // конструктор Page
            {
                LastAccess = DateTime.Now;
                this.startingIndex = startingIndex; this.length = length;

                var r = new Random();                  // сгенерировать 1000 элементов на страницу
                for (int i = 0; i < length; i++)
                    pageData.Add(new Sample
                    {
                        Temp = r.Next(50, 95),
                        Pressure = 28.0 + r.NextDouble() * 4
                    });
            }
            public bool HasItem(int index) =>          // доступность элемента
                ((index >= startingIndex) && (index < startingIndex + length));

            public Sample this[int index]              // индексатор Page
            {
                get
                {
                    LastAccess = DateTime.Now;
                    return pageData[index - startingIndex];
                }
                set
                {
                    Dirty      = true;
                    LastAccess = DateTime.Now;
                    // из индекса 3547 отнимается стартовая позиция 3000
                    // получается 547, входящее в размер pageData = 1000
                    pageData[index - startingIndex] = value;
                }
            }
        }
    }
}