using System;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        // объявлено "2" переменные цикла идентичного типа
        for (int i = 1, j = 20; i < j; i++, j--)
        {
            //TODO
        }




        var li = Enumerable.Range(1, 10).ToList();
        var sb = new StringBuilder();
        // объявлено "2" переменные цикла различных типов, используя синтаксис кортежа
        for ((int i, bool first) = (0, true); i < li.Count; i++)
        {
            if (!first)
                sb.Append(", ");

            sb.Append(li[i]);

            first = false;
        }
        Console.WriteLine(sb.ToString());
        // 1, 2, 3, 4, 5, 6, 7, 8, 9, 10
    }
}
