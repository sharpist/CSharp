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
        static void Main()
        {
            SampleTwo();
        }
    }
}