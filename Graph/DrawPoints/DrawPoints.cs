using System.Drawing;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox.BackColor = Color.Black;
        }
        int i;
        private Graphics g;
        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            g = (sender as Control).CreateGraphics(); // создать холст из контрола
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; // добовить размытие
            g.FillEllipse(new SolidBrush(Color.White), e.X, e.Y, 5, 5); // нарисовать точку

            if (i++ == 3) { i = 0; pictureBox.Refresh(); } // очистить холст
            g.Dispose(); // освободить ресурсы
        }
    }
}
