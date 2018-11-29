# Преобразования
______________________________________________________________

#### Преобразовать ```Int32``` в байт-массив и наоборот: ####
```c#
Int32 inputValue = 255;
Console.WriteLine("входное значение Int32: {0}", inputValue);

Console.WriteLine($"\nInt32 преобразуется в байт-массив...");
byte[] bytes = BitConverter.GetBytes(inputValue);
Console.WriteLine($"значение байт-массива: {BitConverter.ToString(bytes)}");

Console.WriteLine("\nбайт-массив преобразуется в Int32... ");
var outputValue = BitConverter.ToInt32(bytes, 0);
Console.WriteLine("значение Int32: {0}", outputValue);
/* Output:
    входное значение Int32: 255

    Int32 преобразуется в байт-массив...
    значение байт-массива: FF-00-00-00

    байт-массив преобразуется в Int32...
    значение Int32: 255
*/
```

#### Преобразовать ```String``` в байт-массив в UTF-16 и наоборот: ####
```c#
String inputValue = "ABCD";
Console.WriteLine("входное значение String: {0}", inputValue);

Console.WriteLine($"\nString преобразуется в байт-массив...");
byte[] bytes = UnicodeEncoding.Unicode.GetBytes(inputValue);
Console.WriteLine($"значение байт-массива: {BitConverter.ToString(bytes)}");

Console.WriteLine("\nбайт-массив преобразуется в String... ");
var outputValue = UnicodeEncoding.Unicode.GetString(bytes);
Console.WriteLine("значение String: {0}", outputValue);
/* Output:
    входное значение String: ABCD

    String преобразуется в байт-массив...
    значение байт-массива: 41-00-42-00-43-00-44-00

    байт-массив преобразуется в String...
    значение String: ABCD
*/
```
