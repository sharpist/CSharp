using System;

namespace EventPattern
{
    class Program
    {
        static void Main()
        {
            var stock = new Stock("THPW");
            stock.Price = 27.10M;
            stock.PriceChanged += stock_PriceChanged;
            stock.Price = 31.59M;
        }
        static void stock_PriceChanged(object sender, PriceChangedEventArgs e)
        {
            if ((e.NewPrice - e.LastPrice) / e.LastPrice > 0.1M)
                Console.WriteLine("Внимание, увеличение цены на 10%!");
        }
    }

    public class PriceChangedEventArgs : EventArgs // передаёт информацию событию
    {
        public readonly decimal LastPrice, NewPrice;
        public PriceChangedEventArgs(decimal lastPrice, decimal newPrice)
            => (LastPrice, NewPrice) = (lastPrice, newPrice);
    }

    public class Stock
    {
        string symbol; decimal price;
        public Stock(string symbol) => this.symbol = symbol;
        // используется встроенный обобщённый делегат EventHandler<TEventArgs>
        // *делегат EventHandler для событий без информации
        public event EventHandler<PriceChangedEventArgs> PriceChanged;
        // защищённый виртуальный метод запускает событие
        // *параметр EventArgs e для событий без информации
        protected virtual void OnPriceChanged(PriceChangedEventArgs e)
            => PriceChanged?.Invoke(this, e);

        public decimal Price
        {
            get => price;
            set
            {
                if (price == value) return;
                // производный от EventArgs инкапсулирует информацию
                // для передачи, когда инициируется событие PriceChanged
                // *аргумент EventArgs.Empty для событий без информации
                OnPriceChanged(new PriceChangedEventArgs(price, value));
                price = value;
            }
        }
    }
}