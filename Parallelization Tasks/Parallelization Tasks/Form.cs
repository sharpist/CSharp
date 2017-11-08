﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Parallelization_Tasks
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form() { // constructor
            InitializeComponent();

            // вычисляется размер массива
            int dataSize = bytesPerPixel * pixelWidth * pixelHeight;
            data = new byte[dataSize];
        }

        // горизонтальное и вертикальное разрешение для Bitmap
        private int pixelWidth = 700, pixelHeight = 500;

        private Bitmap bmpRGB;
        private Rectangle rect;
        private BitmapData bmpData;

        private int bytesPerPixel = 3; // 1 пиксельное значение в 3 байтах
        private byte[] data;           // байтовый массив для хранения данных графики

        private byte redValue,
                     greenValue,
                     blueValue;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            /// создать "пустой" (all-zeros) 24bpp Bitmap объект для вывода графики
            bmpRGB = new Bitmap(pixelWidth, pixelHeight, PixelFormat.Format24bppRgb);
            /// создать Rectangle и заблокировать растровое изображение в системной памяти
            rect = new Rectangle(0, 0, pixelWidth, pixelHeight);
            bmpData = bmpRGB.LockBits(rect, ImageLockMode.WriteOnly, bmpRGB.PixelFormat);


            // генерирует случайные значения для цветности RGB графики
            Random rand = new Random();
            redValue = (byte)rand.Next(0xFF);
            greenValue = (byte)rand.Next(0xFF);
            blueValue = (byte)rand.Next(0xFF);

            #region RUNNING IN FLOWS
            // запуск таймера
            Stopwatch watch = Stopwatch.StartNew(); // применяется для операций отсчета времени

            // данные для всего изображения находятся между 0 и pixelWidth / 2
            Task first = Task.Run(() => generateGraphData(data, 0, pixelWidth / 8)); // range is specified only for +X
            Task second = Task.Run(() => generateGraphData(data, pixelWidth / 8, pixelWidth / 4));
            Task third = Task.Run(() => generateGraphData(data, pixelWidth / 4, pixelWidth * 3 / 8));
            Task fourth = Task.Run(() => generateGraphData(data, pixelWidth * 3 / 8, pixelWidth / 2));
            Task.WaitAll(first, second, third, fourth); // task synchronization

            // отображение времени, затраченного на создание данных
            duration.Text = $"Время (мс): {watch.ElapsedMilliseconds}";
            #endregion

            #region displaying
            // разблокировка растрового изображения из системной памяти
            bmpRGB.UnlockBits(bmpData);
            // вывести графику в pictureBox
            pictureBox.Image = bmpRGB;
            #endregion
        }

        // заполняет массив данными для графики
        private void generateGraphData(byte[] data, int partitionStart, int partitionEnd)
        {
            int Y = pixelHeight / 2, X = pixelWidth / 2;

            for (short x = (short)partitionStart; x < partitionEnd; x++) // iterations = X
            {
                double p = Math.Sqrt(X * X - x * x);

                for (var i = p; i > -p; i -= 4) // value X for -Y and Y
                {
                    double r = Math.Sqrt(x * x + i * i) / X;
                    double q = (r - 1) * Math.Sin(5 * r);
                    double y = i / 3 + (q * Y);

                    #region plotXY
                    // изображение зеркально относительно оси X поэтому метод plotXY вызывается дважды
                    plotXY(data, (int)(-x + (pixelWidth / 2)), (int)(y + (pixelHeight / 2)));
                    plotXY(data, (int)(x + (pixelWidth / 2)), (int)(y + (pixelHeight / 2)));
                    #endregion
                }
            }
        }

        // устанавливает в массиве - данные байтов для каждой точки,
        // значения байтов соответствуют X и Y координатам
        private void plotXY(byte[] data, int x, int y)
        {
            unsafe
            {
                // присвоить указателю адрес первых пиксельных данных
                byte* ptr = (byte*)bmpData.Scan0;

                // построение пиксела
                int index = (y * pixelWidth + x ) * bytesPerPixel;
                // назначить компонент RGB-значений к указателю
                ptr[index + 2] = redValue;
                ptr[index + 1] = greenValue;
                ptr[index] = blueValue;
            }
        }
    }
}

