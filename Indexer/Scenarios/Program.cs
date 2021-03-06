﻿using IndexersSamples.Common;
using IndexersSamples.SampleThree;
using IndexersSamples.SampleTwo;
using System;

namespace IndexersSamples
{
    class Program
    {
        static void SampleTwo()
        {
            ArgsActions actions;
            actions["-a"] = () => Console.WriteLine("-a option selected");
            actions["-e"] = () => Console.WriteLine("-e option selected");

            var processor = new ArgsProcessor(actions);
            processor.Process("-a", "-e");
            // -a option selected
            // -e option selected
        }

        static void SampleThree()
        {
            HistoricalWeatherData data;

            data["Chicago", new DateTime(1970, 6, 6)] = new Sample
            { Temp = 75, Pressure = 30.2 };

            var sample = data["Chicago", new DateTime(1970, 6, 6)];
            Console.WriteLine(sample.Temp); // 75

            sample = data["Chicago", new DateTime(1970, 6, 6, 12, 30, 2)];
            Console.WriteLine(sample.Temp); // 75


            data["Chicago", new DateTime(1970, 6, 6)] = new Sample
            { Temp = 85, Pressure = 30.2 };

            sample = data["Chicago", new DateTime(1970, 6, 6)];
            Console.WriteLine(sample.Temp); // 85

            sample = data["Chicago", new DateTime(1970, 6, 6, 12, 30, 2)];
            Console.WriteLine(sample.Temp); // 85


            try
            { sample = data["New York", new DateTime(1980, 5, 12)]; }

            catch (ArgumentOutOfRangeException e)
            { Console.WriteLine(e.Message); } // перехвачено исключение

            try
            { sample = data["Chicago", new DateTime(1980, 5, 12)]; }

            catch (ArgumentOutOfRangeException e)
            { Console.WriteLine(e.Message); } // перехвачено исключение
        }
        static void Main()
        {
            SampleTwo();
            SampleThree();
        }
    }
}