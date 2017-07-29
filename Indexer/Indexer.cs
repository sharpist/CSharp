...
    class Program
    {
        ...
            var example = new Example<string>(5);

            example[0] = "Hello";
            example[1] = "World!";
            example[2] = "How";
            example[3] = "Are";
            example[4] = "You?";


            Console.Write($"{example[0]} {example[1]} {example[2]} {example[3]} {example[4]}");
            // Hello World! How Are You?
    }

    struct Example<T>
    {
        private T[] arr;
        public Example(int param)
        { arr = new T[param]; }

        public T this[int i] // индексатор
        {
            get { return this.arr[i]; }
            set { this.arr[i] = value; }
        }
    }














...
    class Program
    {
        ...
            IntBits bits = new IntBits(126); // (126) 0111 1110


            bool peek = bits[6]; // 0[1]11 1110   извлечение булева значения с индексом 6 = true

            bits[0] = true;      // 0111 111[1]   установка бита с индексом 0 в true

            bits[3] = false;     // 0111 [0]111   установка бита с индексом 3 в false

            bits[6] ^= true;     // 0[0]11 0111   инвертирует значение бита с индексом 6 = false

            // теперь в bits содержится значение (55) 0011 0111
    }

    struct IntBits
    {
        private int bits;
        public IntBits(int param)
        { this.bits = param; }
        

        public bool this [int index] // индексатор
        {
            get
            {
                return (bits & (1 << index)) != 0;
            }

            set
            {
                if (value)
                    bits |= (1 << index);
                else
                    bits &= ~(1 << index);
            }
        }
    }

