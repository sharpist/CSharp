using IndexersSamples.Common;
using System;
// псевдоним сконструированного универсального типа с описательным именем
using CityDataSample =
    System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.Dictionary<System.DateTime, IndexersSamples.Common.Sample>>;
using DateSample =
    System.Collections.Generic.Dictionary<System.DateTime, IndexersSamples.Common.Sample>;

namespace IndexersSamples.SampleThree
{
    struct HistoricalWeatherData
    {
        /// <summary>
        /// Моделирование типа двухмерного словаря, образующего внутреннее хранилище,
        /// для управления историческими данными о температуре по двум аргументам:
        /// городу (string) и дате (DateTime).
        /// </summary>
        public Sample this[string city, DateTime date]
        {
            get
            {
                var cityData = default(DateSample);
                // выбрать в хранилище пару ключ-значение,
                // инициализировать словарь cityData
                if (!storage.TryGetValue(city, out cityData))
                    // иначе выбросить исключение
                    throw new ArgumentOutOfRangeException(nameof(city), "City not found");
                // получить дату в качестве ключа
                var index = date.Date;
                var sample = default(Sample);
                return (cityData.TryGetValue(index, out sample)) ?
                    sample :
                    throw new ArgumentOutOfRangeException(nameof(date), "Date not found");
            }
            set
            {
                var cityData = default(DateSample);
                // выбрать в хранилище пару ключ-значение,
                // инициализировать словарь cityData
                if (!storage.TryGetValue(city, out cityData))
                {
                    // иначе создать словарь cityData,
                    cityData = new DateSample();
                    // добавить в хранилище по ключу
                    storage.Add(city, cityData);
                }
                // получить дату в качестве ключа
                var index = date.Date;
                cityData[index] = value;
            }
        }

        static readonly CityDataSample storage;
        static HistoricalWeatherData() => storage = new CityDataSample();
    }
}