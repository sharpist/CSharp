using System;
using System.Drawing;

namespace Peano_GosperCurve
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.Black;
            pen = new Pen(Color.Red, 1);
        }

        private Graphics g;
        private Pen pen;

        private void draw(double x, double y, double l, double u, int t, int q)
        {
            // начало построения ломанных
            if (t > 0)
            {
                if (q == 1)
                {
                    // формулы построения
                    x += l * Math.Cos(u);
                    y -= l * Math.Sin(u);
                    u += Math.PI;
                }
                u -= 2 * Math.PI/19; // соединение линий
                l /= Math.Sqrt(7);   // масштаб
                // функции рисования
                paint(ref x, ref y, l,               u, t-1, 0);
                paint(ref x, ref y, l,   u + Math.PI/3, t-1, 1);
                paint(ref x, ref y, l,     u + Math.PI, t-1, 1);
                paint(ref x, ref y, l, u+2 * Math.PI/3, t-1, 0);
                paint(ref x, ref y, l,               u, t-1, 0);
                paint(ref x, ref y, l,               u, t-1, 0);
                paint(ref x, ref y, l,   u - Math.PI/3, t-1, 1);
            }
            else
            {
                g.DrawLine(pen, (float)Math.Round(x),
                                (float)Math.Round(y),
                                (float)Math.Round(x + Math.Cos(u)*l),
                                (float)Math.Round(y - Math.Sin(u)*l));
            }
        }
        private void paint(ref double x, ref double y, double l, double u, int t, int q)
        {
            draw(x, y, l, u, t, q);
            // формулы построения
            x += l * Math.Cos(u);
            y -= l * Math.Sin(u);
        }


        private void button_Click(object sender, EventArgs e)
        {
            g = Graphics.FromHwnd(pictureBox.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            draw(65, 420, 450, 0, 4, 0); // w, h, scale
        }
    }
}
