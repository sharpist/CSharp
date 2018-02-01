using System;

class Tuples
{
    static void Main(string[] args)
    {
        var myInt    = 50;
        var myString = "Hello";
        var myArray  = new[] {'A', 'B'};


        var tuple = new Tuple<int, string, char[]>(myInt, myString, myArray);
                     //short version: Tuple.Create(myInt, myString, myArray);

        var res = TestMethod(tuple);
        // "50"    -> "100"
        // "Hello" -> "Hello World!"
        // "A B"   -> "C D"
    }


    private static Tuple<int, string, char[]> TestMethod(Object t)
    {
        var t2 = t as Tuple<int, string, char[]>;
        var myInt    = t2.Item1;
        var myString = t2.Item2;
        var myArray  = t2.Item3;


        myInt      = 100;
        myString  += " World!";
        myArray[0] = 'C'; myArray[1] = 'D';

        return Tuple.Create(myInt, myString, myArray); //returned 3 values!
    }
}