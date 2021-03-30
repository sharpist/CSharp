using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

// ШАБЛОН ДЛЯ БАЗОВОГО КЛАССА
public class BaseUnmanagedClass : IDisposable
{
    private IntPtr     buffer;   // неуправляемый буфер памяти
    private SafeHandle resource; // управляемый ресурс

    public BaseUnmanagedClass()
    {
        this.buffer   = Marshal.AllocHGlobal(1024);
        this.resource = new SafeFileHandle(IntPtr.Zero, true);
    }

    ~BaseUnmanagedClass() =>     // если объект не уничтожен до завершения программы
        this.Dispose(false);     // высвобождает неуправляемый ресурс

    public void Dispose()        // высвобождает все ресурсы
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        lock (this) {
            if (this.disposed) return;
            this.disposed = true;

            /* высвобождение неуправляемого ресурса */
            Marshal.FreeHGlobal(buffer);
            buffer = IntPtr.Zero;

            /* высвобождение управляемого ресурса */
            if (disposing)
            {
                resource?.Dispose();
            }
        }
    }
}

// ШАБЛОН ДЛЯ ПРОИЗВОДНОГО КЛАССА
public class DerivedUnmanagedClass : BaseUnmanagedClass
{
    private IntPtr     buffer;   // неуправляемый буфер памяти
    private SafeHandle resource; // управляемый ресурс

    public DerivedUnmanagedClass()
    {
        this.buffer   = Marshal.AllocHGlobal(1024);
        this.resource = new SafeFileHandle(IntPtr.Zero, true);
    }

    ~DerivedUnmanagedClass() =>  // если объект не уничтожен до завершения программы
        this.Dispose(false);     // высвобождает неуправляемый ресурс

    private bool disposed = false;

    protected override void Dispose(bool disposing)
    {
        lock (this) {
            if (this.disposed) return;
            this.disposed = true;

            /* высвобождение неуправляемого ресурса */
            Marshal.FreeHGlobal(buffer);
            buffer = IntPtr.Zero;

            /* высвобождение управляемого ресурса */
            if (disposing)
            {
                resource?.Dispose();
            }
            base.Dispose(disposing);
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
