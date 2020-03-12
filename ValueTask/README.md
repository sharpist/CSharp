# Caching **async** ops with **ValueTask**  

*В случаях, когда метод с модификатором **async** возвращает кэшированный результат, или выполняется синхронно, дополнительное выделение памяти в куче может занимать значительное время в критических секциях кода.*

---

Возможности *C#* позволяют возвращать из асинхронных методов другие типы, кроме ```void```, ```Task``` и ```Task<T>```.  
Например, тип **```ValueTask```**, который доступен в *.NET Core*:

```csharp
public async ValueTask<int> Func()
{
    await Task.Delay(1000);
    return 777;
}
```

**Для дополнительной оптимизации можно кэшировать результаты асинхронной работы и использовать эти результаты в последующих вызовах**.

У структуры **```ValueTask```** есть конструктор принимающий ```Task``` в качестве параметра, так что можно создать **```ValueTask```** из возвращаемого значения любого существующего асинхронного метода:

```csharp
using System.Threading.Tasks;
using static System.Console;

namespace CachingAsyncOps
{
    class CachingAsync
    {
        async Task<int> loadCache()
        {
            // симуляция асинх. работы
            await Task.Delay(1000);

            cacheResult = 777;
            cache = true;
            return cacheResult;
        }
        int  cacheResult;
        bool cache;

        public ValueTask<int> CachedFunc() => cache ?
            new ValueTask<int>(cacheResult) : new ValueTask<int>(loadCache());
    }

    class Program
    {
        static void Main() => WriteLine(new CachingAsync()
            .CachedFunc().Result); // output: 777
    }
}
```
