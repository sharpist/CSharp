using System;
using System.Threading;
using System.Threading.Tasks;

namespace Events
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
            this.order     = new PlaceOrder();
            this.procedure = new ImportantProcedure();

            // добавляет к делегату Processing объекта типа PlaceOrder
            // ссылку на метод Audit объекта типа ImportantProcedure
            this.order.Processing += this.procedure.Audit;
            // подписка на событие
            this.procedure.AuditComplete += this.Message;
        }

        private PlaceOrder order;             // ссылка на экземпляр класса PlaceOrder
        private ImportantProcedure procedure; // ссылка на экземпляр класса ImportantProcedure


        private void Message(string message) // метод Message сообщает о завершении важной операции
        { // он вызывается событием, для гарантии критерия: важная операция выполнена 
            label.Invoke((Action)(() => { label.Text = message; }));
        }

        private async void button_Click(object sender, EventArgs e)
        {
            this.label.Text = "Procedure in progress...";
            await Task.Run(() => this.order.StartProcessing());
        }
    }


    class PlaceOrder
    {
        public delegate void ProcessingDelegate(); // делегат
        public ProcessingDelegate Processing;      // хранит ссылку на метод Audit

        public void StartProcessing()
        {
            this.Processing?.Invoke(); // запустить метод делегата
        }
    }

    class ImportantProcedure
    {
        public delegate void AuditCompleteDelegate(string message); // делегат
        public event AuditCompleteDelegate AuditComplete;           // событие

        public async void Audit()
        {
            try
            {
                // await ...
                Thread.Sleep(2500); // импровизированная работа
            }
            catch (Exception ex) { }

            finally
            {   // инициировать событие AuditComplete по завершении выполнения метода Audit
                this.AuditComplete?.Invoke("The Event Happened!\n" + "criterion: Procedure Completed");
            }
        }
    }
}
