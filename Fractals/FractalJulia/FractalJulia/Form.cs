using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FractalJulia
{
    public partial class Form : System.Windows.Forms.Form
    {
        private (byte[], byte[], byte[]) compileFractal(int w, int h, (byte[] r, byte[] g, byte[] b) arrays) 
        {
            /// на каждой итерации вычисляется znew = zold² + C

            double cRe, cIm;                       // вещественная и мнимая части постоянной C
            double newRe, newIm, oldRe, oldIm;     // вещественная и мнимая части старой и новой

            double zoom = 1, moveX = 0, moveY = 0; // размер, положение

            ushort maxIterations = 300;            // предел итераций для прекращения функции


            // значения константы C определяют форму фрактала Жюлиа
            cRe = -0.70176;
            cIm = -0.3842;


            for (int x = 0, index = 0; x < w; x++) // проходим каждый пиксель
                for (int y = 0; y < h; y++, index++)
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

                    // определить цвета в массивах для хранения компонентов RGB
                    arrays.r[index] = (byte)((i * 9) % 255);
                    arrays.g[index] = 0;
                    arrays.b[index] = (byte)((i * 9) % 255);
                }
            return arrays;
        }

        private void displayFractal(int w, int h, (byte[] r, byte[] g, byte[] b) arrays)
        {
            const byte bytesPerPixel = 4; // 1 пиксельное значение в 4 байтах

            // создать "пустой" (all-zeros) 32bpp Bitmap объект для вывода графики
            using (var bmpRGB = new Bitmap(w, h, PixelFormat.Format32bppArgb))
            {
                // создать Rectangle и заблокировать растровое изображение в системной памяти
                var rect = new Rectangle(0, 0, w, h);
                var bmpData = bmpRGB.LockBits(rect, ImageLockMode.WriteOnly, bmpRGB.PixelFormat);

                unsafe
                {
                    // присвоить указателю адрес первых пиксельных данных
                    byte* ptr = (byte*)bmpData.Scan0;

                    for (int x = 0, index = 0; x < w; x++)
                        for (int y = 0; y < h; y++, index++)
                        {
                            // построение пиксела 
                            var p = (y * w + x) * bytesPerPixel;
                            // назначить компонент RGB-значений к указателю
                            ptr[p + 3] = 0xBF;
                            ptr[p + 2] = arrays.r[index];
                            ptr[p + 1] = arrays.g[index];
                            ptr[p] = arrays.b[index];
                        }
                }

                // разблокировка растрового изображения из системной памяти
                bmpRGB.UnlockBits(bmpData);

                // вывести графику в pictureBox
                pictureBox.Image = null;
                using (var ms = new MemoryStream())
                {
                    bmpRGB.Save(ms, ImageFormat.Bmp);
                    pictureBox.Image = Image.FromStream(ms);
                }
            }
        }


        private void button_Click(object sender, EventArgs e)
        {
            int w = pictureBox.Width,
                h = pictureBox.Height;

            var arrays = compileFractal(w, h, (new byte[w * h], new byte[w * h], new byte[w * h]));
                         displayFractal(w, h, arrays);
        }
        private void Form1_Load(object sender, EventArgs e) { /**/ }
        public Form() { InitializeComponent(); }
    }
}
