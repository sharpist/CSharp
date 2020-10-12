// ШАБЛОН ДЛЯ БАЗОВОГО КЛАССА

    public class Example : IDisposable
    {
        private IntPtr buffer;         // неуправляемый буфер памяти
        private SafeHandle resource;   // управляемый ресурс
        public Example()               // конструктор
        {
            this.buffer = ...
            this.resource = ...
        }


        private bool disposed = false; // флаг

        ~Example()                     // деструктор
        { this.Dispose(false); }       // высвобождает неуправляемый ресурс (запуск деструктора, если объект не уничтожен до завершения программы)

        public virtual void Dispose()  // высвобождает все ресурсы
        {
            this.Dispose(true);
            GC.SuppressFinalize(this); // не вызывать метод завершения (предотвратить запуск деструктора)
        }
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!this.disposed)
                {
                    this.disposed = true;
                    ReleaseBuffer(buffer); // высвобождение неуправляемого ресурса
                    if (disposing)
                    {
                        if (resource != null) resource.Dispose(); // высвобождение управляемого ресурса
                    }
                }
            }
        }
    }

/*
Так как IDisposable полагается на явный вызов метода Dispose программистом (поскольку в рантайме он сам вызываться не будет),
можно добавить финализатор/деструктор в свой класс для обеспечения автоматической, гарантированной очистки ресурсов:
в таком случае даже, если забыть о методе Dispose, GC всё сделает сам.

ПРОИЗВОДНЫЙ КЛАСС, от класса, реализующего интерфейс IDisposable, не должен реализовывать интерфейс IDisposable,
поскольку реализация метода IDisposable.Dispose базового класса наследуется производными классами.

Необходимо:
    Перегрузить override метод protected `` void Dispose(Boolean) чтобы он также вызывал метод Dispose(Boolean) базового класса
    и передавал в него значение true. Для этого включить в метод последней инструкцию base.Dispose(disposing).

    Рекомендуется переопределить деструктор для производного класса + он должен вызывать перегруженный метод Dispose(false).
*/
