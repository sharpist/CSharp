using Composite.Component;
using System;

namespace Composite.Leaf
{
    // Leaf
    public class SingleGift : GiftBase
    {
        public SingleGift(string name, int price)
            : base(name, price)
        {
        }

        public override int CalculateTotalPrice()
        {
            Console.WriteLine($"{name} стоимостью {price}");

            return price;
        }
    }
}
