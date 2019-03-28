using System;

namespace ComplexNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            // код создает два Complex-объекта,
            // комплексные значения (10 + 4i) и (5 + 2i)
            Complex first = new Complex(10, 4), second = new Complex(5, 2);

            Console.WriteLine($"first is {first}");
            Console.WriteLine($"second is {second}");


            Console.WriteLine($"\n{first} + {second} = {first + second}");
            Console.WriteLine($"{first} - {second} = {first - second}");
            Console.WriteLine($"{first} * {second} = {first * second}");
            Console.WriteLine($"{first} / {second} = {first / second}");


            Console.WriteLine($"\nImplicit conversion Int -> Complex:\n{first} += 2 = {first += 2}");
            int tempInt = (int)first;
            Console.WriteLine($"\nInt value after conversion:\nint tempInt = (int)first;\ntempInt = {tempInt}");

            /*
            first is (10 + 4i)
            second is (5 + 2i)

            (10 + 4i) + (5 + 2i) = (15 + 6i)
            (10 + 4i) - (5 + 2i) = (5 + 2i)
            (10 + 4i) * (5 + 2i) = (42 + 40i)
            (10 + 4i) / (5 + 2i) = (2 + 0i)

            Implicit conversion Int -> Complex:
            (10 + 4i) += 2 = (12 + 4i)

            Int value after conversion:
            int tempInt = (int)first;
            tempInt = 12
            */
        }
    }

    class Complex
    {
        public int Real { get; set; }      // представляет вещественную часть
        public int Imaginary { get; set; } // представляет мнимую часть

        public Complex(int real, int imaginary) {
            this.Real = real;
            this.Imaginary = imaginary;
        }
        public Complex(int real) {         // конструктор для неявного преобразования
            this.Real = real;
            this.Imaginary = 0;
        }
        public override string ToString() => $"({this.Real} + {this.Imaginary}i)";


        // оператор неявного преобразования
        public static implicit operator Complex(int from) => new Complex(from);
        // оператор явного преобразования
        public static explicit operator int(Complex from) => from.Real;

        /*
        Операция                Вычисление
        (a + bi) + (c + di)     ((a + c) + (b + d)i)

        (a + bi) – (c + di)     ((a – c) + (b – d)i)

        (a + bi)(c + di)        ((ac – bd) + (bc + ad)i)

        (a + bi)/(c + di)       (((ac + bd)/(cc + dd)) + ((bc – ad)/(cc + dd))i)
        */

        // перегружаемый оператор +
        public static Complex operator +(Complex lhs, Complex rhs) // получает два Complex-объекта, складывает
        {                                                          // и возвращает новый Complex-объект
            return new Complex(lhs.Real + rhs.Real, lhs.Imaginary + rhs.Imaginary);
        }
        // перегружаемый оператор -
        public static Complex operator -(Complex lhs, Complex rhs)
        {
            return new Complex(lhs.Real - rhs.Real, lhs.Imaginary - rhs.Imaginary);
        }
        // перегружаемый оператор *
        public static Complex operator *(Complex lhs, Complex rhs)
        {
            return new Complex(
                lhs.Real * rhs.Real - lhs.Imaginary * rhs.Imaginary,
                lhs.Imaginary * rhs.Real + lhs.Real * rhs.Imaginary );
        }
        // перегружаемый оператор /
        public static Complex operator /(Complex lhs, Complex rhs)
        {
            int realElement =
                (lhs.Real * rhs.Real + lhs.Imaginary * rhs.Imaginary) /
                (rhs.Real * rhs.Real + rhs.Imaginary * rhs.Imaginary);

            int imaginaryElement =
                (lhs.Imaginary * rhs.Real - lhs.Real * rhs.Imaginary) /
                (rhs.Real * rhs.Real + rhs.Imaginary * rhs.Imaginary);

            return new Complex(realElement, imaginaryElement);
        }

        // перегружаемый оператор ==
        public static bool operator ==(Complex lhs, Complex rhs) => lhs.Equals(rhs);
        // перегружаемый оператор !=
        public static bool operator !=(Complex lhs, Complex rhs) => !(lhs.Equals(rhs));
        ///
        public override bool Equals(Object obj)
        {
            if (obj is Complex)
            {
                Complex compare = (Complex)obj;
                return (this.Real == compare.Real) &&
                (this.Imaginary == compare.Imaginary);
            }
            else return false;
        }
        public override int GetHashCode()
        {
            // возвращает хеш-код зависящий от поля/полей объекта
            return Convert.ToInt32(Math.Pow((Real + Imaginary) ^ 17457, 2) % 10000);
        }
    }
}
