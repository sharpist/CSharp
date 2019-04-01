using System;
using System.Runtime.InteropServices;
using System.Text;
/// <summary>
/// *опция /unsafe
/// утилита командной строки читает указанный в командной строке
/// текстовый файл и отображает в консоли его содержимое
/// </summary>
class FileReader
{
    const uint GENERIC_READ = 0x80000000;
    const uint OPEN_EXISTING = 3;
    IntPtr handle;

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
            IntPtr hFile,             // handle файла
            void* pBuffer,            // буфер данных
            int NumberOfBytesToRead,  // количество байт для чтения
            int* pNumberOfBytesRead,  // количество прочитанных байт
            int Overlapped            // вместо указателя на структуру overlapped используется int
            );
    [DllImport("kernel32", SetLastError = true)]
    static extern unsafe bool CloseHandle(
            IntPtr hObject            // дескриптор объекта
            );


    public bool Open(string FileName)
    {
        // открыть существующий файл для чтения
        handle = CreateFile(FileName, GENERIC_READ, 0, 0, OPEN_EXISTING, 0, 0);
        return (handle != IntPtr.Zero) ? true : false;
    }

    public unsafe int Read(byte[] buffer, int index, int count)
    {
        int n = 0;
        fixed (byte* p = buffer)
        {
            if (!ReadFile(handle, p + index, count, &n, 0))
                return 0;
        }
        return n;
    }

    public bool Close() => CloseHandle(handle);
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


        var buffer = new byte[128];
        var reader = new FileReader();

        if (reader.Open(args[0]))
        {
            // предполагается чтение файла в кодировке UTF-8
            var encoding = new UTF8Encoding();
            try
            {
                int bytesRead;
                do
                {
                    bytesRead = reader.Read(buffer, 0, buffer.Length);
                    var content = encoding.GetString(buffer, 0, bytesRead);
                    Console.Write("{0}", content);
                }
                while (bytesRead > 0);
            }
            catch { }
            finally
            {
                reader.Close();
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
