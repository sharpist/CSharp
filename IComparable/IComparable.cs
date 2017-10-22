using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class TestType : IComparable<TestType>
    {
        public int ID { get; set; }

        public override string ToString() => $"ID: {this.ID}";


        int IComparable<TestType>.CompareTo(TestType other)
        {
            if (other == null) // сравнивает объекты на основе поля ID
                return 1;

            if (this.ID > other.ID)
                return 1;

            if (this.ID < other.ID)
                return -1;

            return 0;
        }
    }
}
