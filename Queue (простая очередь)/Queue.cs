...
    class Program
    {
        ...
            Queue<string> myQueue = new Queue<string>(3);
            // 1. principle: "who came first, leaves first"
            // 2. with expandable on demand
            // 3. the extracted item will be removed from the queue
            // 4. also the item can be deleted on demand
            myQueue.Add("First");
            myQueue.Add("Second");
            myQueue.Add("Third");
            Console.WriteLine(myQueue.Get());     // "First"
            Console.WriteLine(myQueue.Get());     // "Second"
            Console.WriteLine(myQueue.Get());     // "Third"

            myQueue.Add("Sixth");
            myQueue.Add("Fifth");
            myQueue.Add("Fourth");

            myQueue.Del(3);                       // "Fourth" now removed
            myQueue.Sort();                       // "Sixth", "Fifth" -> "Fifth", "Sixth" (abc...)
            Console.WriteLine(myQueue.HowMany()); // "2"
            Console.WriteLine(myQueue.Get());     // "Fifth"
            Console.WriteLine(myQueue.Get());     // "Sixth"
    }

    class Queue<T> where T : IComparable<T>
    {
        private byte element;                    // флаг кол-во элементов
        private T[] arr;
        public Queue() {
            this.element = 0;
            this.arr = new T[this.element + 1];
        }
        public Queue(byte size) {
            this.element = 0;
            this.arr = new T[this.element + size];
        }

        public void Add(T item)                  // добавить элемент
        {
            if (this.element == this.arr.Length) resizeArray(1);
            this.arr[this.element++] = item;
        }
        public T Get()                           // извлечь элемент
        {
            if (this.element == 0) throw new IndexOutOfRangeException("The queue is empty!");
            var item = this.arr[0];
            resizeArray(0);
            this.element--;

            return item;
        }
        public void Del(byte index)              // удалить элемент
        {
            if (this.element < index) throw new IndexOutOfRangeException("The queue does not carry this item!");
            resizeArray(-1, index);
            this.element--;
        }
        public int HowMany() => this.arr.Length; // показать список
        public void Sort()                       // сортировать элементы
        {
            if (this.arr.Length > 0) resizeArray(2);
        }

        #region вспомогательный метод
        private void resizeArray(sbyte param, byte index = 0)
        {
            switch (param)
            {
                case 1 :  // увеличить массив (добавление элемента)
                T[] buf = new T[this.arr.Length];

                for (int i = 0; i < this.arr.Length; i++)
                { buf[i] = this.arr[i]; }

                this.arr = new T[this.arr.Length + 1];

                for (int i = 0; i < buf.Length; i++)
                { this.arr[i] = buf[i]; }
                break;

                case 0 :  // уменьшить массив (извлечение элемента)
                buf = new T[this.arr.Length - 1];

                for (int i = 0; i < this.arr.Length - 1; i++)
                { buf[i] = this.arr[i + 1]; }

                this.arr = new T[buf.Length];
                this.arr = buf;
                break;

                case -1 : // уменьшить массив (удаление элемента)
                buf = new T[this.arr.Length - 1];
                int j = 0;
                for (int i = 0; i < this.arr.Length; i++)
                { if (i != index - 1) buf[j++] = this.arr[i]; }

                this.arr = buf;
                break;

                case 2 :  // сортировать элементы
                for (int min = 0; min < this.arr.Length - 1; min++) {
                    for (int max = min + 1; max < this.arr.Length; max++)
                    {
                        if (this.arr[min].CompareTo(this.arr[max]) >= 0)
                        {
                            var temp = this.arr[min];
                            this.arr[min] = this.arr[max];
                            this.arr[max] = temp;
                        }
                    }
                }
                break;
            }
        }
        #endregion
    }
