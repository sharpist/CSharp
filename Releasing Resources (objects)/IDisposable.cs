// ШАБЛОН ДЛЯ БАЗОВОГО КЛАССА

public class UnmanagedClass : System.IDisposable
{
    // неуправляемый буфер памяти
    private System.IntPtr buffer;
    // управляемый ресурс
    private System.Runtime.InteropServices.SafeHandle resource;

    public UnmanagedClass()
    {
        this.buffer = System.Runtime.InteropServices.Marshal.AllocHGlobal(1024);
        //this.resource = ...
    }


    private bool disposed = false;

    ~UnmanagedClass()             // если объект не уничтожен до завершения программы
    { this.Dispose(false); }      // высвобождает неуправляемый ресурс

    public virtual void Dispose() // высвобождает все ресурсы
    {
        this.Dispose(true);
        System.GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        lock (this) {
            if (this.disposed) return;
            this.disposed = true;

            /* высвобождение неуправляемого ресурса */

            System.Runtime.InteropServices.Marshal.FreeHGlobal(buffer);
            buffer = System.IntPtr.Zero;

            /* высвобождение управляемого ресурса */
            if (disposing)
            {
                if (resource is not null) resource.Dispose();
            }
        }
    }
}

/*
IDisposable полагается на явный вызов метода Dispose программистом в блоке finally (в рантайме он сам вызываться не будет),
поэтому можно добавить деструктор (финализатор) в свой класс для обеспечения автоматической, гарантированной очистки ресурсов:
в таком случае даже, если забыть о методе Dispose, GC всё сделает сам.

ПРОИЗВОДНЫЙ КЛАСС, от класса, реализующего интерфейс IDisposable, не должен реализовывать интерфейс IDisposable,
поскольку реализация метода IDisposable.Dispose базового класса наследуется производными классами.

Необходимо:
	Перегрузить override метод protected `` void Dispose(Boolean) чтобы он также вызывал метод Dispose(Boolean) базового класса
	и передавал в него значение true. Для этого включить в метод последней инструкцию base.Dispose(disposing).

	Рекомендуется переопределить деструктор для производного класса + он должен вызывать перегруженный метод Dispose(false).
*/
