using System;
using System.Drawing;

namespace BezierCurve
{
    public partial class Form : System.Windows.Forms.Form
    {
        private Graphics g;                   // объект графики
        private PointF[] arr = new PointF[5]; // исходный массив точек


        private void draw() // функция рисования кривой
        {
            int j = 0;
            float step = 0.01f; // возьмем шаг 0.01 для большей точности

            PointF[] result = new PointF[101]; // конечный массив точек кривой
            for (float t = 0; t < 1; t += step)
            {
                float ytmp = 0;
                float xtmp = 0;
                for (int i = 0; i < arr.Length; i++)
                { // проходим по каждой точке
                    float b = polinom(i, arr.Length - 1, t); // вычисляем наш полином Бернштейна
                    xtmp += arr[i].X * b; // записываем и прибавляем результат
                    ytmp += arr[i].Y * b;
                }
                result[j] = new PointF(xtmp, ytmp);
                j++;
            }
            g.DrawLines(new Pen(Color.Red, 2), result); // рисуем полученную кривую Безье
        }


        private float polinom(int i, int n, float t) // функция вычисления полинома Бернштейна
            => (factorial(n) / (factorial(i) * factorial(n - i))) * (float)Math.Pow(t, i) * (float)Math.Pow(1 - t, n - i);

        private int factorial(int n) // функция вычисления факториала
        {
            int res = 1;
            for (int i = 1; i <= n; i++)
                res *= i;
            return res;
        }



        
        public Form() {
            InitializeComponent();
            pictureBox.BackColor = Color.Black;
        }
        private void button_Click(object sender, EventArgs e)
        {
            g = Graphics.FromHwnd(pictureBox.Handle);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            axisXY();
            graph();
            draw();
        }
        private void axisXY()
        {
            int w = pictureBox.Width, h = pictureBox.Height;

            for (int i = 30; i < 480; i += 30) {
                g.DrawLine(new Pen(Color.FromArgb(0,0,51), 1), i, 0, i, h);
                g.DrawLine(new Pen(Color.FromArgb(0,0,51), 1), 0, i, w, i);
            }
            g.DrawLine(new Pen(Color.Indigo, 2), w/2,   0, w/2,   h);
            g.DrawLine(new Pen(Color.Indigo, 2),   0, h/2,   w, h/2);
        }
        private void graph()
        {
            float a = -2, c = 2;
            for (int pX = -2; pX <= 2; pX++)
            {
                float pY = a*(pX*pX) + c;
                arr[pX+2] = new PointF(240+pX*30, 180-pY*40);
            }
        }
    }
}
