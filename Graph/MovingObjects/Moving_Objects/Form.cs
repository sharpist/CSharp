using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace Moving_Objects
{
    public partial class Form : System.Windows.Forms.Form
    {
        readonly int size = 5; // количество точек
        private byte interval; // флаг интервала отрисовки


        private async Task StartShifting(CancellationToken token)
        {
            Task task = Task.Factory.StartNew(() =>
            Parallel.For(0, points.Count, new ParallelOptions { CancellationToken = token },
            (x) => Shifting(x, token)), token/*, TaskCreationOptions.LongRunning*/);

            try
            {
                await task;
            }
            catch (OperationCanceledException oce)
            { label.Text = $"{oce.Message}\nStatus: '{task.Status}'"; }
            /*
            finally
            { if (cts != null) cts.Dispose(); }
            */

            //label.Text = $"Status: '{task.Status}'";
        }

        // построить координаты
        private void Shifting(int index, CancellationToken token)
        {
            bool north, south, east, west;
            Checking(out north, out south, out east, out west, index);
            var vectors = Vectors(ref north, ref south, ref east, ref west) as Tuple<short, short>;

            for (int i = r.Next(10, 20); i > 0; i--) // инерция векторов
            {
                token.ThrowIfCancellationRequested(); // создать исключение при запросе на отмену

                points.Add(index, ref vectors);

                Checking(out north, out south, out east, out west, index);
                if (north || south || east || west)
                    vectors = Vectors(ref north, ref south, ref east, ref west) as Tuple<short, short>;

                Displaying(ref vectors);
                Thread.Sleep(20);
            }
        }

        // получить векторы
        private Tuple<short, short> Vectors(ref bool north, ref bool south, ref bool east, ref bool west)
        {
            // векторы движения (coordinate + vector)
            short motionVectorX = 0, motionVectorY = 0;

            while (true)
            { // задать вектор X
                motionVectorX = (short)r.Next(-1, 2);

                if (east) {
                    if (motionVectorX < 1) break;
                    else continue;
                }
                if (west) {
                    if (motionVectorX > -1) break;
                    else continue;
                }
                break;
            }

            while (true)
            { // задать вектор Y
                motionVectorY = (short)r.Next(-1, 2);

                if (north) {
                    if (motionVectorY < 1) break;
                    else continue;
                }
                if (south) {
                    if (motionVectorY > -1) break;
                    else continue;
                }
                break;
            }

            return Tuple.Create(motionVectorX, motionVectorY);
        }

        // регистрировать отскок
        private void Checking(out bool north, out bool south, out bool east, out bool west, int index)
        {
            if (points.Read(index).Y >=  225) north = true; else north = false;
            if (points.Read(index).Y <= -225) south = true; else south = false;
            if (points.Read(index).X >=  225) east  = true; else east  = false;
            if (points.Read(index).X <= -225) west  = true; else west  = false;
        }

        // обновить экран
        private void Refreshing()
        {
            if (pictureBox.InvokeRequired)
            {
                Action action = () => { pictureBox.Refresh(); };
                pictureBox.Invoke(action); // вызываем эту же функцию обновления состояния, но уже в UI-потоке
            }
            else pictureBox.Refresh(); // код обновления состояния контрола
        }

        // отрисовать графику
        private void Displaying(ref Tuple<short, short> vectors)
        {
            if (interval++ == 1)
            {
                if (interval == 2)
                {
                    Refreshing(); interval = 0;
                }
                for (int index = 0; index < points.Count; index++) {
                    lock (this.g) {
                        Sprites(225 + points.Read(index).X, 225 + points.Read(index).Y, index);
                    }
                }
            }
        }

        // выбрать спрайты
        private void Sprites(float X, float Y, int index)
        {
            // рисовать спрайты
            imageList.Draw(g, new Point((int)X-16, (int)Y-16), 0); // x, y, #image

            // рисовать маркеры
            //g.FillEllipse(new SolidBrush(Color.FromArgb(0, 255, 0)),
                //225 + points.Read(index).X, 225 + points.Read(index).Y, 3, 3);

            // вывести таймер
            g.DrawString($"Time {watch.Elapsed.Minutes}:{watch.Elapsed.Seconds}",
                new Font("Arial", 14), new SolidBrush(Color.Black), new PointF(0.0F, 425.0F));
        }


        private async void buttonStart_Click(object sender, EventArgs e)
        {
            label.Text = "";

            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            watch = Stopwatch.StartNew();
            while (!token.IsCancellationRequested)
            {
                await StartShifting(token);
            }
            watch.Stop();
        }
        private void buttonStop_Click(object sender, EventArgs e)
        {
            if (cts != null) cts.Cancel(); // инициировать отмену задачи
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //TODO
        }

        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.FromArgb(200, 200, 200);

            g = Graphics.FromHwnd(pictureBox.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            r = new Random();
            points = new SynchronizedCache(size);

            // генерировать координаты точек
            for (int index = 0; index < size; index++)
            {
                var vectors = Tuple.Create((short)r.Next(-225, 225), (short)r.Next(-225, 225));
                points.Add(index, ref vectors);
            }

            //timer.Enabled = true; // активировать таймер
        }
        private Random    r;
        private Graphics  g;
        private Stopwatch watch;
        private SynchronizedCache points;
        private CancellationTokenSource cts = null;
    }
}
