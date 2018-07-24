using static System.Console;

class Program
{
    private static void AccessEmbeddedArray()
    {
        var myC = new MyClass();
        unsafe
        {
            // оператор fixed не позволяет сборщику мусора
            // переносить перемещаемую переменную

            // закрепить буфер в фиксированном месте в памяти
            fixed (char* charPtr = myC.myBuffer.fixedBuffer)
            {
                *charPtr = 'A';
            }

            // безопасный доступ через индекс
            char c = myC.myBuffer.fixedBuffer[0];
            WriteLine(c);

            // изменить через индекс
            myC.myBuffer.fixedBuffer[0] = 'B';
            WriteLine(myC.myBuffer.fixedBuffer[0]);

            myC.myBuffer.fixedBuffer[1] = 'N';
            WriteLine(myC.myBuffer.fixedBuffer[1]);
        }
        // A
        // B
        // N
    }

    internal unsafe class MyClass
    {
        public MyBuffer myBuffer = default;
    }
    internal unsafe struct MyBuffer
    {
        // оператор fixed можно использовать для
        // создания буферов фиксированного размера
        public fixed char fixedBuffer[128];
        // размер массива из 128 элементов char составляет 256 байт
        // в буферах типа char фиксированного размера на один символ
        // всегда приходится два байта независимо от кодировки
    }


    static void Main() => AccessEmbeddedArray();
}

