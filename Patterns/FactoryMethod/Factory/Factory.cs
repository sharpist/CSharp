using FactoryMethod.Functionality;

namespace FactoryMethod.Factory
{
    // реализация классов и методов фабрики
    public class CoolingFactory : AirConditionerFactory
    {
        public override IAirConditioner Create(double temperature)
            => new Cooling(temperature);
    }

    public class WarmingFactory : AirConditionerFactory
    {
        public override IAirConditioner Create(double temperature)
            => new Warming(temperature);
    }
}
