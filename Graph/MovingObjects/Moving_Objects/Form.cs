﻿using System;
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


        // построить координаты
        private void shifting(int index)
        {
            bool north, south, east, west;
            checking(out north, out south, out east, out west, index);
            var vectors = Vectors(north, south, east, west) as Tuple<short, short>;

            for (int i = r.Next(10, 20); i > 0; i--) // инерция векторов
            {
                points.Add(index, vectors);

                checking(out north, out south, out east, out west, index);
                if (north || south || east || west)
                    vectors = Vectors(north, south, east, west) as Tuple<short, short>;

                displaying();
                Thread.Sleep(2);
            }
        }

        // получить векторы
        private Tuple<short, short> Vectors(bool north, bool south, bool east, bool west)
        {
            // векторы движения (coordinate + vector)
            short motionVectorX = 0, motionVectorY = 0;

            do { // задать вектор X
                motionVectorX = (short)r.Next(-1, 2);
                if (east) {
                    if (motionVectorX == 1) continue;
                }
                if (west) {
                    if (motionVectorX == -1) continue;
                }
                if (motionVectorX == 0) continue;
                else break;
            } while (true);

            do { // задать вектор Y
                motionVectorY = (short)r.Next(-1, 2);
                if (north) {
                    if (motionVectorY == 1) continue;
                }
                if (south) {
                    if (motionVectorY == -1) continue;
                }
                if (motionVectorY == 0) continue;
                else break;
            } while (true);

            return Tuple.Create(motionVectorX, motionVectorY);
        }

        // регистрировать отскок
        private void checking(out bool north, out bool south, out bool east, out bool west, int index)
        {
            if (points.Read(index).Y ==  225) north = true; else north = false;
            if (points.Read(index).Y == -225) south = true; else south = false;
            if (points.Read(index).X ==  225) east  = true; else east  = false;
            if (points.Read(index).X == -225) west  = true; else west  = false;
        }

        // обновить экран
        private void refreshing()
        {
            if (pictureBox.InvokeRequired)
            {
                Action action = () => { pictureBox.Refresh(); };
                pictureBox.Invoke(action); // вызываем эту же функцию обновления состояния, но уже в UI-потоке
            }
            else pictureBox.Refresh(); // код обновления состояния контрола
        }

        // отрисовать графику
        private void displaying()
        {
            if (interval++ == 1)
            {
                if (interval == 2)
                {
                    refreshing(); interval = 0;
                }
                for (int index = 0; index < points.Count; index++)
                {
                    lock (this.g)
                    {
                        // вывести графику
                        g.FillEllipse(new SolidBrush(Color.FromArgb(0, 255, 0)),
                        225 + points.Read(index).X, 225 + points.Read(index).Y, 3, 3);
                        // вывести таймер
                        g.DrawString($"Time: {watch.Elapsed.Minutes}:{watch.Elapsed.Seconds}",
                            new Font("Arial", 14), new SolidBrush(Color.White), new PointF(0.0F, 425.0F));
                    }
                }
            }
        }


        private async void button_Click(object sender, EventArgs e)
        {
            watch = Stopwatch.StartNew();

            Task task = Task.Run(() =>
            Parallel.For(0, points.Count, shifting));
            await task;

            //watch.Stop();
        }
        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.FromArgb(25, 25, 25);

            g = Graphics.FromHwnd(pictureBox.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            r = new Random();
            points = new SynchronizedCache(size);

            // генерировать координаты точек
            for (int index = 0; index < size; index++)
                points.Add(index, Tuple.Create((short)r.Next(-225, 225), (short)r.Next(-225, 225)));
        }
        private Random    r;
        private Graphics  g;
        private Stopwatch watch;
        private SynchronizedCache points;
    }
}
