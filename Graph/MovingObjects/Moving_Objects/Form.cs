using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace Moving_Objects
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Random    r;
        private Graphics  g;
        private Stopwatch watch;
        private SynchronizedCache points;
        readonly int size = 5; // количество точек
        private byte interval; // флаг интервала отрисовки


        // перемещать точки
        private void startMoving()
        {
            bool north, south, east, west;
            while (true)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    checkPosition(out north, out south, out east, out west, i);
                    var vectors = setVectors(north, south, east, west) as Tuple<short, short>;

                    for (int j = r.Next(10, 20); j > 0; j--) // инерция векторов
                    {
                        points.Add(i, vectors);

                        checkPosition(out north, out south, out east, out west, i);
                        if (north || south || east || west)
                            vectors = setVectors(north, south, east, west) as Tuple<short, short>;

                        draw();
                        Thread.Sleep(3);
                    }
                }
            }
        }

        // отрисовать графику
        private void draw()
        {
            if (interval++ == 1) {
                if (interval == 2) {
                    interval = 0; pictureBox.Refresh(); // обновить экран
                }
                for (int index = 0; index < points.Count; index++) {
                    g.FillEllipse(new SolidBrush(Color.FromArgb(0, 255, 0)),
                        225 + points.Read(index).X, 225 + points.Read(index).Y, 3, 3);
                    // вывести таймер
                    g.DrawString($"Time: {watch.Elapsed.Minutes}:{watch.Elapsed.Seconds}", new Font("Arial", 14), new SolidBrush(Color.White), new PointF(0.0F, 425.0F));
                }
            }
        }

        // регистрировать отскок
        private void checkPosition(out bool north, out bool south, out bool east, out bool west, int index)
        {
            if (points.Read(index).Y ==  225) north = true; else north = false;
            if (points.Read(index).Y == -225) south = true; else south = false;
            if (points.Read(index).X ==  225) east  = true; else east  = false;
            if (points.Read(index).X == -225) west  = true; else west  = false;
        }

        // получить векторы
        private Tuple<short, short> setVectors(bool north, bool south, bool east, bool west)
        {
            // векторы движения (coordinate + vector)
            short motionVectorX = 0, motionVectorY = 0;

            do
            { // задать вектор X
                motionVectorX = (short)r.Next(-1, 2);
                if (east)
                {
                    if (motionVectorX == 1) continue;
                }
                if (west)
                {
                    if (motionVectorX == -1) continue;
                }
                if (motionVectorX == 0) continue;
                else break;
            } while (true);

            do
            { // задать вектор Y
                motionVectorY = (short)r.Next(-1, 2);
                if (north)
                {
                    if (motionVectorY == 1) continue;
                }
                if (south)
                {
                    if (motionVectorY == -1) continue;
                }
                if (motionVectorY == 0) continue;
                else break;
            } while (true);

            return Tuple.Create(motionVectorX, motionVectorY);
        }


        private void button_Click(object sender, EventArgs e)
        {
            watch = Stopwatch.StartNew();
            startMoving();
            watch.Stop();
        }
        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.FromArgb(25, 25, 25);

            g = Graphics.FromHwnd(pictureBox.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            r = new Random();
            points = new SynchronizedCache(size);

            // генерировать координаты точек
            for (int i = 0; i < size; i++)
                points.Add(i, Tuple.Create((short)r.Next(-225, 225), (short)r.Next(-225, 225)));
        }
    }
}
