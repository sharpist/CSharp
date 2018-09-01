using System;
using System.Drawing;
using System.Windows.Forms;

namespace FractalJulia
{
    public partial class Form : System.Windows.Forms.Form
    {
        public void DrawFractal(int w, int h, Graphics g, Pen pen) 
        {
            /// на каждой итерации вычисляется znew = zold² + C

            double cRe, cIm;                       // вещественная и мнимая части постоянной C
            double newRe, newIm, oldRe, oldIm;     // вещественная и мнимая части старой и новой

            double zoom = 1, moveX = 0, moveY = 0; // размер, положение

            ushort maxIterations = 300;            // предел итераций для прекращения функции


            // значения константы C определяют форму фрактала Жюлиа
            cRe = -0.70176;
            cIm = -0.3842;


            for (int x = 0; x < w; x++)            // проходим каждый пиксель
                for (int y = 0; y < h; y++)
                {
                    // вычисляется реальная и мнимая части числа z
                    // на основе расположения пикселей, масштабирования и значения позиции
                    newRe = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX;
                    newIm =       (y - h / 2) / (0.5 * zoom * h) + moveY;

                    int i = 0;                     // число итераций
                    while (i < maxIterations)      // начинается процесс итерации
                    {
                        // сохранить значение предыдущей итерации
                        oldRe = newRe;
                        oldIm = newIm;

                        // в текущей итерации вычисляются действительная и мнимая части 
                        newRe = oldRe * oldRe - oldIm * oldIm + cRe;
                        newIm = 2 * oldRe * oldIm + cIm;

                        // если точка находится вне круга с радиусом 2 - прервать
                        if ((newRe * newRe + newIm * newIm) > 4) break;
                        i++;
                    }


                    pen.Color = Color.FromArgb(255, (i * 9) % 255, 0, (i * 9) % 255); // определить цвета
                    g.DrawRectangle(pen, x, y, 1, 1);                                 // нарисовать пиксель
                }
        }

        private void button_Click(object sender, EventArgs e)
        {
            var pen = new Pen(Color.Black, 1);                                        // перо myPen черного цвета, толщиной в 1 пиксель
            var g   = Graphics.FromHwnd(pictureBox.Handle);                           // объект g с возможностью рисовать в pictureBox

            DrawFractal(845, 475, g, pen);
        }

        public Form()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
