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

/*
ПРОИЗВОДНЫЙ КЛАСС, от класса, реализующего интерфейс IDisposable, не должен реализовывать интерфейс IDisposable,
поскольку реализация метода IDisposable.Dispose базового класса наследуется производными классами.

Необходимо:
    Перегрузить override метод protected``Dispose(Boolean) чтобы он также вызывал метод Dispose(Boolean) базового класса
    и передавал в него значение true. Для этого включить в метод последней инструкцию base.Dispose(disposing). 
*/