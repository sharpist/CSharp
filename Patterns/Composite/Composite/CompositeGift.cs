using Composite.Component;
using System;
using System.Collections.Generic;

namespace Composite.Composite
{
    // Composite
    public class CompositeGift : GiftBase, IGiftOperations
    {
        private List<GiftBase> _gifts;

        public CompositeGift(string name, int price)
            : base(name, price)
        {
            _gifts = new List<GiftBase>();
        }

        public override int CalculateTotalPrice()
        {
            Console.WriteLine($"{name} содержит следующие товары:");

            var total = 0;
            foreach (var gift in _gifts)
                total += gift.CalculateTotalPrice();

            return total;
        }

        public void Add(GiftBase gift) => _gifts.Add(gift);
        public void Remove(GiftBase gift) => _gifts.Remove(gift);
    }
}
