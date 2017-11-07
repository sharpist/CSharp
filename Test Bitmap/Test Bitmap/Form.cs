using System;
using System.Drawing;

namespace Test_Bitmap
{
    public partial class Form : System.Windows.Forms.Form
    {
        private byte[] byteArray;

        public Form()
        {
            InitializeComponent();

            labelInfo.Text = "";
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            // загрузить изображение в byte array
            using (var memoryStream = new System.IO.MemoryStream())
            {
                //Image image = Image.FromFile("image.png"); // из файла
                Image image = Properties.Resources.image;  // из ресурса
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] byteArray = memoryStream.ToArray();

                #region скопировать данные в поле byte array
                this.byteArray = new byte[byteArray.Length];
                for (int i = 0; i < byteArray.Length; ++i)
                    this.byteArray[i] = byteArray[i];
                #endregion
            }
            // byte array в bitmap
            using (var memoryStream = new System.IO.MemoryStream(this.byteArray))
            {
                var bitmap = new System.Drawing.Bitmap(memoryStream);
                pictureBox.Image = bitmap;
            }

            labelInfo.Text = "image uploaded!";
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.byteArray != null)
            {
                // преобразовать в bmp, записать на диск
                using (var memoryStream = new System.IO.MemoryStream())
                {
                    foreach (byte b in this.byteArray) memoryStream.WriteByte(b);
                    Image image = Image.FromStream(memoryStream);
                    image.Save("newImage.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                }

                labelInfo.Text = "image is converted to BMP and saved!";
            }
            else
                labelInfo.Text = "need to download the image!";
        }
    }
}
