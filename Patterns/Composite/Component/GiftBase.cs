namespace Composite.Component
{
    // Component
    public abstract class GiftBase
    {
        // поля и метод используются в качестве интерфейса
        // между Leaf и Composite
        protected string name;
        protected int price;

        public GiftBase(string name, int price)
        {
            this.name = name;
            this.price = price;
        }

        public abstract int CalculateTotalPrice();
    }
}
