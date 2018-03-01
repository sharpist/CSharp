using System;
using System.Diagnostics;
using System.Drawing;

namespace Visualized_Sorting
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Random r;
        private Graphics g;
        private Stopwatch watch;
        private PointF[] points;
        readonly int size = 175;

        private byte visualize;
        private byte refresh;


        private void sorting()
        {
            for (byte i = 0; i < points.Length - 1; i++)
            {
                byte f = 0;
                for (byte j = 0; j < points.Length - i - 1; j++)
                {
                    if (points[j].X > points[j + 1].X)
                    {
                        var buf = points[j + 1].X;
                        points[j + 1].X = points[j].X;
                        points[j].X = buf;
                        f = 1;
                        if (visualize++ == 1) {
                            refresh++;
                            draw(); // отрисовать
                        }
                    }
                }
                visualize = 0;
                if (f == 0) break;
            }

            for (byte i = 0; i < points.Length - 1; i++)
            {
                byte f = 0;
                for (byte j = 0; j < points.Length - i - 1; j++)
                {
                    if (points[j].Y > points[j + 1].Y)
                    {
                        var buf = points[j + 1].Y;
                        points[j + 1].Y = points[j].Y;
                        points[j].Y = buf;
                        f = 1;
                        if (visualize++ == 1) {
                            refresh++;
                            draw(); // отрисовать
                        }
                    }
                }
                visualize = 0;
                if (f == 0) break;
            }
        }


        private void draw()
        {
            if (refresh > 1) {
                refresh = 0; pictureBox.Refresh();
            }
            for (int i = 0; i < points.Length; i++) {
                g.FillEllipse(new SolidBrush(Color.FromArgb(0, 255, 0)),
                    225 + points[i].X, 225 + points[i].Y, 3, 3);
                // вывести таймер
                g.DrawString($"Time: {watch.Elapsed.Minutes}:{watch.Elapsed.Seconds}", new Font("Arial", 14), new SolidBrush(Color.White), new PointF(0.0F, 425.0F));
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            watch = Stopwatch.StartNew();
            sorting();
            watch.Stop();
        }
        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.FromArgb(25, 25, 25);

            g               = Graphics.FromHwnd(pictureBox.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            r      = new Random();
            points = new PointF[size];
            // генерировать координаты точек
            for (int i = 0; i < size; i++)
                points[i] = new PointF(r.Next(-225, 225), r.Next(-225, 225));
        }
    }
}
