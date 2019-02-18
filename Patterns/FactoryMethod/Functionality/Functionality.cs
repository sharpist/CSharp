using System;

namespace FactoryMethod.Functionality
{
    // базовая функциональность
    public class Cooling : IAirConditioner
    {
        readonly double _temperature;

        public Cooling(double temperature)
        {
            _temperature = temperature;
        }

        public void Operate()
        {
            Console.WriteLine($"Cooling the room to the required temperature of {_temperature} degrees.");
        }
    }

    public class Warming : IAirConditioner
    {
        readonly double _temperature;

        public Warming(double temperature)
        {
            _temperature = temperature;
        }

        public void Operate()
        {
            Console.WriteLine($"Warming the room to the required temperature of {_temperature} degrees.");
        }
    }
}
