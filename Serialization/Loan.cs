using System;
using System.ComponentModel;

namespace Serialization
{
    [Serializable()] // всё находящееся в классе может быть сохранено в файле
    public class Loan : INotifyPropertyChanged
    {
        public double LoanAmount { get; set; }
        public double InterestRate { get; set; }

        [field: NonSerialized()] // не подлежит сериализации резервное поле автоматически реализуемого свойства
        public DateTime TimeLastLoaded { get; set; }

        public int Term { get; set; }

        private string customer;
        public string Customer
        {
            get { return customer; }
            set
            {
                customer = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs(nameof(Customer)));
            }
        }

        [field: NonSerialized()] // не подлежит сериализации так как не представляет часть графа объекта,
                                 // иначе будут сериализованы все объекты, присоединенные к этому событию
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public Loan(double loanAmount,
                    double interestRate,
                    int term,
                    string customer)
        {
            this.LoanAmount = loanAmount;
            this.InterestRate = interestRate;
            this.Term = term;
            this.customer = customer;
        }
    }
}
