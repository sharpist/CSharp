...
    class Program
    {
        ...
            Queue<string> myQueue = new Queue<string>(3);
            // 1. principle: "who came first, leaves first"
            // 2. with expandable on demand

            myQueue.Add("First");
            myQueue.Add("Second");
            myQueue.Add("Third");
            Console.WriteLine(myQueue.Get());  // First
            Console.WriteLine(myQueue.Get());  // Second

            myQueue.Add("Fourth");
            Console.WriteLine(myQueue.Get());  // Third
            Console.WriteLine(myQueue.Get());  // Fourth
    }

    class Queue<T>
    {
        private byte element;                  // флаг кол-во элементов
        private T[] arr;

        public Queue() {
            this.element = 0;
            this.arr = new T[this.element + 1];
        }
        public Queue(byte size) {
            this.element = 0;
            this.arr = new T[this.element + size];
        }

        public void Add(T item)                // добавить элемент
        {
            if (this.element == this.arr.Length) resizeArray(true);
            this.arr[this.element++] = item;
        }
        public T Get()                         // извлечь элемент
        {
            if (this.element == 0) throw new IndexOutOfRangeException("The queue is empty!");
            var item = this.arr[0];
            resizeArray(false);
            this.element--;

            return item;
        }

        private void resizeArray(bool param)   // вспомогательный метод
        {
            switch (param)
            {
                case true:                     // увеличить массив
                T[] buf = new T[this.arr.Length];

                for (int i = 0; i < this.arr.Length; i++)
                { buf[i] = this.arr[i]; }

                this.arr = new T[this.arr.Length + 1];

                for (int i = 0; i < buf.Length; i++)
                { this.arr[i] = buf[i]; }
                break;

                case false:                    // уменьшить массив
                buf = new T[this.arr.Length - 1];

                for (int i = 0; i < this.arr.Length - 1; i++)
                { buf[i] = this.arr[i + 1]; }

                this.arr = new T[buf.Length];
                this.arr = buf;
                break;
            }
        }
    }
