using FactoryMethod.ManagementActions;

namespace FactoryMethod
{
    class Program
    {
        static void Main()
        {
            /// <summary>
            /// "Factory Method" - это шаблон проектирования, который предоставляет
            /// интерфейс для создания объектов без указания их конкретных классов.
            /// 
            /// Он определяет метод, который можно использовать для создания объекта
            /// вместо использования его конструктора. Важно то, что подклассы могут
            /// переопределять этот метод и создавать объекты разных типов.
            /// </summary>
            var factory = new AirConditioner().ExecuteCreation(Actions.Cooling, 22.5);
            factory.Operate();
            /* Output:
                Cooling the room to the required temperature of 22,5 degrees.
            */
        }
    }
}
