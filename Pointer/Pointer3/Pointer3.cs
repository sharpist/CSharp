using System;
using System.Runtime.InteropServices;
using System.Text;
/// <summary>
/// *опция /unsafe
/// FileReader – утилита командной строки, которая читает указанный в
/// командной строке текстовый файл и отображает в консоли его содержимое.
/// 
/// Представляет практический пример использования указателей и методов
/// с внешней реализацией для вызова неуправляемого кода.
/// </summary>
class FileReader
{
    const uint GENERIC_READ = 0x80000000;
    const uint OPEN_EXISTING = 3;
    static IntPtr handle;

    [DllImport("kernel32", SetLastError = true)]
    static extern unsafe IntPtr CreateFile(
            string FileName,          // имя файла
            uint DesiredAccess,       // режим доступа
            uint ShareMode,           // режим общего использования
            uint SecurityAttributes,  // атрибуты безопасности
            uint CreationDisposition, // как создавать
            uint FlagsAndAttributes,  // атрибуты файла
            int hTemplateFile         // handle для шаблона файла
            );
    [DllImport("kernel32", SetLastError = true)]
    static extern unsafe bool ReadFile(
            IntPtr hFile,             // дескриптор файла
            void* pBuffer,            // буфер данных
            int NumberOfBytesToRead,  // количество байт для чтения
            int* pNumberOfBytesRead,  // количество прочитанных байт
            int Overlapped            // асинхронный буфер
            );
    [DllImport("kernel32", SetLastError = true)]
    static extern unsafe bool CloseHandle(
            IntPtr hObject            // дескриптор объекта
            );


    public static bool Open(string FileName)
    {
        // открыть существующий файл для чтения
        handle = CreateFile(FileName, GENERIC_READ, 0, 0, OPEN_EXISTING, 0, 0);
        return (handle != IntPtr.Zero) ? true : false;
    }

    public static unsafe int Read(ReadOnlySpan<byte> buffer, int index, int count)
    {
        int n = 0;
        fixed (byte* p = buffer)
        {
            if (!ReadFile(handle, p + index, count, &n, 0))
                return 0;
        }
        return n;
    }

    public static bool Close() => CloseHandle(handle);
}

class Program
{
    public static int Main(string[] args)
    {
        // проверить аргументы
        if (args.Length == 0)
        {
            Console.WriteLine("\nНе указан ни один аргумент:\n" +
                "C:\\>исполняемый_файл \"текстовый_файл\"");
            return 1;
        }
        if (!System.IO.File.Exists(args[0]))
        {
            Console.WriteLine("\nФайл {0} не найден", args[0]);
            return 1;
        }
        // показать выбранные аргументы
        Console.Write("\nАргументы командной строки: ");
        for (int i = 0; i < args.Length; i++)
            Console.Write("[{0}]{1}", args[i],
                                      (i + 1 < args.Length) ? ", " : "\n\n");


        if (FileReader.Open(args[0]))
        {
            try
            {
                // предполагается чтение файла в кодировке UTF-8
                Console.OutputEncoding = Encoding.UTF8;
                Decoder decoder = Encoding.UTF8.GetDecoder();

                int nBytes;
                do
                {
                    ReadOnlySpan<byte> buffer = stackalloc byte[128];
                    nBytes = FileReader.Read(buffer, 0, buffer.Length);

                    // определить число символов в последовательности байтов
                    int nChars = decoder.GetCharCount(buffer, false);
                    Span<char> chars = stackalloc char[nChars];

                    // хранить конечные байты блока данных
                    nChars = decoder.GetChars(buffer, chars, false);
                    Console.Write(chars.ToString());
                }
                while (nBytes > 0);
            }
            catch { }
            finally
            {
                FileReader.Close();
            }
            return 0;
        }
        else
        {
            Console.WriteLine("Не удалось открыть запрашиваемый файл");
            return 1;
        }
    }
}