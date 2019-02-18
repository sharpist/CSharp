using FactoryMethod.Factory;
using FactoryMethod.Functionality;
using System.Collections.Generic;

namespace FactoryMethod.ManagementActions
{
    // перечисление
    public enum Actions { Cooling, Warming }

    // класс, в котором пользователь может указать тип действия
    public class AirConditioner
    {
        readonly Dictionary<Actions, AirConditionerFactory> _factories;

        public AirConditioner()
        {
            _factories = new Dictionary<Actions, AirConditionerFactory>
            {
                { Actions.Cooling, new CoolingFactory() },
                { Actions.Warming, new WarmingFactory() }
            };
        }
        public IAirConditioner ExecuteCreation(Actions action, double temperature)
            => _factories[action].Create(temperature);
    }
}