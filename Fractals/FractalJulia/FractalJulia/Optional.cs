using System;
using System.Windows.Forms;

namespace FractalJulia
{
    public partial class Form
    {
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("All sources on GitHub:\n" +
                @"https://github.com/sharpist/C_Sharp/tree/master/Fractals/FractalJulia",
                "Fractal Julia About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
