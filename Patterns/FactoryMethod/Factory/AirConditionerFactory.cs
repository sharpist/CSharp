using FactoryMethod.Functionality;

namespace FactoryMethod.Factory
{
    // абстрактный класс предоставляет интерфейс для создания объектов в производных классах
    public abstract class AirConditionerFactory
    {
        public abstract IAirConditioner Create(double temperature);
    }
}
