using Composite.Composite;
using Composite.Leaf;
using System;

namespace Composite
{
    class Program
    {
        static void Main()
        {
            var box = new CompositeGift("Коробка", 0);
            var innerBox = new CompositeGift("Внутренняя коробка", 0);

            // товары
            var phone = new SingleGift("Телефон", 184);
            phone.CalculateTotalPrice();

            var magicCube = new SingleGift("Кубик Рубика", 34);
            var gameConsole = new SingleGift("Игровая приставка", 429);
            var chess = new SingleGift("Шахматы", 22);

            box.Add(magicCube);
            box.Add(gameConsole);
            innerBox.Add(chess);
            box.Add(innerBox);

            Console.WriteLine();
            Console.WriteLine($"\nОбщая стоимость подарков: {box.CalculateTotalPrice()}");
            /* Output:
                Телефон стоимостью 184

                Коробка содержит следующие товары:
                Кубик Рубика стоимостью 34
                Игровая приставка стоимостью 429
                Внутренняя коробка содержит следующие товары:
                Шахматы стоимостью 22

                Общая стоимость подарков: 485
            */
        }
    }
}
