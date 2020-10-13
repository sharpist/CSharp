public class Program
{
    public static void Main() {
        // код создает два Complex-объекта,
        // комплексные значения (10 + 4i) и (5 + 2i)
        Complex first = new Complex(10, 4), second = new Complex(5, 2);
        System.Console.WriteLine($"first is {first}, second is {second}\n");

        System.Console.WriteLine($"{first} + {second} = {first + second}");
        System.Console.WriteLine($"{first} - {second} = {first - second}");
        System.Console.WriteLine($"{first} * {second} = {first * second}");
        System.Console.WriteLine($"{first} / {second} = {first / second}");

        System.Console.WriteLine($"\nImplicit conversion Int -> Complex:\n{first} += 2 = {first += 2}");
        int value = (int)first;
        System.Console.WriteLine($"\nInt value after conversion:\nint value = (int)first; // {value}");

        /* Output:
            first is (10 + 4i), second is (5 + 2i)

            (10 + 4i) + (5 + 2i) = (15 + 6i)
            (10 + 4i) - (5 + 2i) = (5 + 2i)
            (10 + 4i) * (5 + 2i) = (42 + 40i)
            (10 + 4i) / (5 + 2i) = (2 + 0i)

            Implicit conversion Int -> Complex:
            (10 + 4i) += 2 = (12 + 4i)

            Int value after conversion:
            int value = (int)first; // 12
        */
    }
}

public class Complex
{
    public int Real      { get; set; } // представляет вещественную часть
    public int Imaginary { get; set; } // представляет мнимую часть

    public Complex(int real, int imaginary) =>
        (this.Real, this.Imaginary) = (real, imaginary);
    // конструктор для неявного преобразования
    public Complex(int real) => (this.Real, this.Imaginary) = (real, 0);


    // оператор неявного преобразования
    public static implicit operator Complex(int from) => new Complex(from);
    // оператор явного преобразования
    public static explicit operator int(Complex from) => from.Real;

    /*       Операция:              Вычисление:
        (a + bi) + (c + di)    ((a + c) + (b + d)i)
        (a + bi) – (c + di)    ((a – c) + (b – d)i)
        (a + bi) * (c + di)    ((ac – bd) + (bc + ad)i)
        (a + bi) / (c + di)    (((ac + bd)/(cc + dd)) + ((bc – ad)/(cc + dd))i)
    */

    // перегрузки операторов +, -, * и /
    public static Complex operator +(Complex lhs, Complex rhs) =>        // получает два Complex-объекта, складывает
        new Complex(lhs.Real + rhs.Real, lhs.Imaginary + rhs.Imaginary); // и возвращает новый Complex-объект

    public static Complex operator -(Complex lhs, Complex rhs) =>
        new Complex(lhs.Real - rhs.Real, lhs.Imaginary - rhs.Imaginary);

    public static Complex operator *(Complex lhs, Complex rhs) =>
        new Complex(
            lhs.Real * rhs.Real - lhs.Imaginary * rhs.Imaginary,
            lhs.Imaginary * rhs.Real + lhs.Real * rhs.Imaginary);

    public static Complex operator /(Complex lhs, Complex rhs) {
        int realElement =
            (lhs.Real * rhs.Real + lhs.Imaginary * rhs.Imaginary) /
            (rhs.Real * rhs.Real + rhs.Imaginary * rhs.Imaginary);

        int imaginaryElement =
            (lhs.Imaginary * rhs.Real - lhs.Real * rhs.Imaginary) /
            (rhs.Real * rhs.Real + rhs.Imaginary * rhs.Imaginary);

        return new Complex(realElement, imaginaryElement);
    }

    // перегрузки операторов == и !=
    public static bool operator ==(Complex lhs, Complex rhs) => lhs.Equals(rhs);

    public static bool operator !=(Complex lhs, Complex rhs) => !(lhs.Equals(rhs));


    public override string ToString() => $"({this.Real} + {this.Imaginary}i)";

    public override bool Equals(System.Object obj)
    {
        if (obj is Complex) {
            var compare = (Complex)obj;
            return
                (this.Real == compare.Real) && (this.Imaginary == compare.Imaginary);
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked {
            int value1 = this.Real.GetHashCode(),
                value2 = this.Imaginary.GetHashCode();

            var shift = 2 & 0x1F;
            var number = System.BitConverter.ToUInt32(System.BitConverter.GetBytes(value1), 0);
            var wrapped = number >> (32 - shift);

            value1 = System.BitConverter.ToInt32(System.BitConverter.GetBytes((number << shift) | wrapped), 0);
            return value1 ^ value2;
        }
    }
}
