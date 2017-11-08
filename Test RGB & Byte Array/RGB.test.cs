using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Test_RGB
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form() {
            InitializeComponent();

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            // определить размеры изображения
            int width = 800, height = 600;

            // создать 3 массива для хранения компонентов RGB
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            // создать объект Random и заполнить массивы RGB случайными байтами
            Random rand = new Random();
            rand.NextBytes(r);
            rand.NextBytes(g);
            rand.NextBytes(b);

            // создать "пустой" (all-zeros) 24bpp Bitmap объект
            Bitmap bmpRGB = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            // создать Rectangle и заблокировать растровое изображение в системной памяти
            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData bmpData = bmpRGB.LockBits(rect, ImageLockMode.WriteOnly, bmpRGB.PixelFormat);

            // рассчитать заполнение для 24bpp растрового изображения
            int padding = bmpData.Stride - 3 * width;
            unsafe
            {
                // присвоить указателю адрес первых пиксельных данных
                byte* ptr = (byte*)bmpData.Scan0;
                for(int y = 0; y < height; y++)
                {
                    for(int x = 0; x < width; x++)
                    {
                        // назначить компонент RGB-значений к указателю
                        ptr[2] = r[y * width + x];
                        ptr[1] = g[y * width + x];
                        ptr[0] = b[y * width + x];

                        // переместить указатель на следующие пиксельные данные
                        ptr += 3;
                    }
                    // заполнение каждой строки
                    ptr += padding;
                }
            }

            // разблокировка растрового изображения из системной памяти
            bmpRGB.UnlockBits(bmpData);

            // сохранить растровое изображение как BMP-файл в папке выполнения
            bmpRGB.Save("rgb_test.bmp", ImageFormat.Bmp);
        }
    }
}
