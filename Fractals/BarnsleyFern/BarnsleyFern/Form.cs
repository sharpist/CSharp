using System;
using System.Drawing;
using System.Threading;

namespace BarnsleyFern
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.Black;
            r = new Random();
        }
        private void buttonDraw_Click(object sender, EventArgs e) {
            g = pictureBox.CreateGraphics();
            g.FillRectangle(Brushes.Black, 0, 0, pictureBox.Width, pictureBox.Height);
            Draw(280, 548, 175, Math.PI / 2); // x, y, scale, rotation
        }


        private Random r;
        private Graphics g;

        private void Draw(int x, int y, double a, double b)
        {
            if (a > 1)
            {
                Line(x, y, a, b);
                x = (int)Math.Round(x + a * Math.Cos(b));
                y = (int)Math.Round(y - a * Math.Sin(b));
                Draw(x, y, a * 0.4, b - 14 * Math.PI / 30);
                Draw(x, y, a * 0.4, b + 14 * Math.PI / 30);
                Draw(x, y, a * 0.7, b + Math.PI / 30);
            }
        }
        private void Line(int x, int y, double a, double b)
        {
            Thread.Sleep(1); // задержка
            g.DrawLine(new Pen(Color.FromArgb(0, r.Next(75, 255), 0), 2),
                x, y, (int)Math.Round(x + a * Math.Cos(b)), (int)Math.Round(y - a * Math.Sin(b)));
        }
    }
}
