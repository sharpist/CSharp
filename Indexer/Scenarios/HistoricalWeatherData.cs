using IndexersSamples.Common;
using System;
using CityDataSample =
    System.Collections.Generic.Dictionary<System.String, System.Collections.Generic.Dictionary<System.DateTime, IndexersSamples.Common.Sample>>;
using DateSample =
    System.Collections.Generic.Dictionary<System.DateTime, IndexersSamples.Common.Sample>;

namespace IndexersSamples.SampleThree
{
    struct HistoricalWeatherData
    {
        public Sample this[string city, DateTime date]
        {
            get
            {
                var cityData = default(DateSample);

                if (!storage.TryGetValue(city, out cityData))
                    throw new ArgumentOutOfRangeException(nameof(city), "City not found");

                // извлечь любую часть времени
                var index = date.Date;
                var sample = default(Sample);
                if (cityData.TryGetValue(index, out sample))
                    return sample;
                throw new ArgumentOutOfRangeException(nameof(date), "Date not found");
            }
            set
            {
                var cityData = default(DateSample);

                if (!storage.TryGetValue(city, out cityData))
                {
                    cityData = new DateSample();
                    storage.Add(city, cityData);
                }

                // извлечь любую часть времени
                var index = date.Date;
                cityData[index] = value;
            }
        }

        static readonly CityDataSample storage;
        static HistoricalWeatherData() => storage = new CityDataSample();
    }
}