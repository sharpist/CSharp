using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class TestType : IComparable<TestType>
    {
        public int Id { get; set; }


        int IComparable<TestType>.CompareTo(TestType other)
        {
            if (other == null) // сравнивает объекты на основе поля Id
                return 1;

            if (this.Id > other.Id)
                return 1;

            if (this.Id < other.Id)
                return -1;

            return 0;
        }
    }
}
