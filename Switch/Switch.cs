using System;
/// Предложение when определяет дополнительное условие,
/// которому должен соответствовать оператор case.
/// Предложение when может быть любым выражением, возвращающим bool значение.
///
/// Часто используется чтобы запретить выполнение раздела switch,
/// если выражение соответствия имеет значение null.

public abstract class Shape // фигура
{
    public abstract double Area { get; }
    public abstract double Circumference { get; }
}

public class Rectangle : Shape // прямоугольник
{
    public Rectangle(double length, double width)
    {
        Length = length; Width = width;
    }
    public double Length { get; }
    public double Width { get; }

    public override double Area
        => Math.Round(Length * Width, 2);

    public override double Circumference
        => (Length + Width) * 2;
}

public class Square : Rectangle // квадрат
{
    public Square(double side) : base(side, side)
    {
        Side = side;
    }
    public double Side { get; }
}


public class Example
{
    public static void Main()
    {
        Shape sh = null;
        Shape[] shapes = { sh, new Square(10),
            new Rectangle(5, 7), new Rectangle(8, 8)};

        foreach (var shape in shapes)
                ShowShapeInfo(shape);
    }
    static void ShowShapeInfo(Shape sh)
    {
        switch (sh) {
            case Shape shape when shape == null:
                // не будет выполнено
                // для проверки на null используется case null:
                break;

            case null:
                Console.WriteLine($"неинициализированное значение");
                break;

            case Square sq when sh.Area > 0:
                Console.WriteLine($"длина стороны: {sq.Side}, площадь: {sq.Area}");
                break;

            case Rectangle r when r.Length == r.Width && r.Area > 0:
                Console.WriteLine($"длина стороны: {r.Length}, площадь: {r.Area}");
                break;

            case Rectangle r when sh.Area > 0:
                Console.WriteLine($"размеры: {r.Length} x {r.Width}, площадь: {r.Area}");
                break;


            default:
                Console.WriteLine($"{nameof(sh)} переменная не представляет собой форму");
                break;
        }
    }
}
// неинициализированное значение
// длина стороны: 10, площадь: 100
// размеры: 5 x 7, площадь: 35
// длина стороны: 8, площадь: 64
