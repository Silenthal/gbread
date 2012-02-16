using System.Drawing;
using System.Windows.Forms;

namespace GBRead
{
    public partial class LineSeparator : UserControl
    {
        public LineSeparator()
        {
            InitializeComponent();
        }

        private void LineSeparator_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.DarkGray, 0, 0, this.Width, 0);
            e.Graphics.DrawLine(Pens.White, 0, 1, this.Width, 1);
        }
    }
}
