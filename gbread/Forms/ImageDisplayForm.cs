using GBRead.Base;
using GBRead.Base.Annotation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GBRead
{
    public partial class ImageDisplayForm : Form
    {
        // TODO: Fix the Image Display
        private BinFile bin;

        private Image igt;

        // TOOD: Use the data label's palette directly.
        private GBPalette gbs;

        private DataLabel dst;
        private int labelOff;
        private int labelLen;

        public ImageDisplayForm(BinFile dt, DataLabel ds)
        {
            InitializeComponent();
            dst = ds;
            bin = dt;
            gbs = ds.Palette;
            labelOff = ds.Offset;
            labelLen = ds.Length;
            SetColors();
            InitializeImage();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            dst.Palette = gbs;
        }

        private void SetColors()
        {
            color1RedBox.SelectedIndex = gbs.Color_1_Red / 8;
            color1GreenBox.SelectedIndex = gbs.Color_1_Green / 8;
            color1BlueBox.SelectedIndex = gbs.Color_1_Blue / 8;
            color2RedBox.SelectedIndex = gbs.Color_2_Red / 8;
            color2GreenBox.SelectedIndex = gbs.Color_2_Green / 8;
            color2BlueBox.SelectedIndex = gbs.Color_2_Blue / 8;
            color3RedBox.SelectedIndex = gbs.Color_3_Red / 8;
            color3GreenBox.SelectedIndex = gbs.Color_3_Red / 8;
            color3BlueBox.SelectedIndex = gbs.Color_3_Red / 8;
            color4RedBox.SelectedIndex = gbs.Color_4_Red / 8;
            color4GreenBox.SelectedIndex = gbs.Color_4_Green / 8;
            color4BlueBox.SelectedIndex = gbs.Color_4_Blue / 8;
            color1Panel.BackColor = Color.FromArgb(gbs.Color_1_Red, gbs.Color_1_Green, gbs.Color_1_Blue);
            color2Panel.BackColor = Color.FromArgb(gbs.Color_2_Red, gbs.Color_2_Green, gbs.Color_2_Blue);
            color3Panel.BackColor = Color.FromArgb(gbs.Color_3_Red, gbs.Color_3_Green, gbs.Color_3_Blue);
            color4Panel.BackColor = Color.FromArgb(gbs.Color_4_Red, gbs.Color_4_Green, gbs.Color_4_Blue);
        }

        private void InitializeImage()
        {
            Color[] palette = new Color[4]
            {
                Color.FromArgb(gbs.Color_1_Red, gbs.Color_1_Green, gbs.Color_1_Blue),
                Color.FromArgb(gbs.Color_2_Red, gbs.Color_2_Green, gbs.Color_2_Blue),
                Color.FromArgb(gbs.Color_3_Red, gbs.Color_3_Green, gbs.Color_3_Blue),
                Color.FromArgb(gbs.Color_4_Red, gbs.Color_4_Green, gbs.Color_4_Blue)
            };
            List<Bitmap> tileList = GBImage.renderImageTiles(bin, labelOff, labelLen, palette);
            igt = GBImage.ConstructImageFromTileList(widthTrackBar.Value, heightTrackBar.Value, scaleBar.Value, tileList);
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (igt != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                Rectangle picRect = pictureBox1.ClientRectangle;
                e.Graphics.DrawImage(igt, picRect, 0, 0, igt.Width, igt.Height, GraphicsUnit.Pixel);
                pictureBox1.Width = igt.Width;
                pictureBox1.Height = igt.Height;
            }
        }

        private void widthTrackBar_Scroll(object sender, System.EventArgs e)
        {
            InitializeImage();
        }

        #region Changing color based on box values

        private void color1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color1RedBox.SelectedIndex != -1 && color1GreenBox.SelectedIndex != -1 && color1BlueBox.SelectedIndex != -1)
            {
                gbs.Color_1_Red = color1RedBox.SelectedIndex * 8;
                gbs.Color_1_Green = color1GreenBox.SelectedIndex * 8;
                gbs.Color_1_Blue = color1BlueBox.SelectedIndex * 8;
                color1Panel.BackColor = Color.FromArgb(gbs.Color_1_Red, gbs.Color_1_Green, gbs.Color_1_Blue);
                InitializeImage();
            }
        }

        private void color2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color2RedBox.SelectedIndex != -1 && color2GreenBox.SelectedIndex != -1 && color2BlueBox.SelectedIndex != -1)
            {
                gbs.Color_2_Red = color2RedBox.SelectedIndex * 8;
                gbs.Color_2_Green = color2GreenBox.SelectedIndex * 8;
                gbs.Color_2_Blue = color2BlueBox.SelectedIndex * 8;
                color2Panel.BackColor = Color.FromArgb(gbs.Color_2_Red, gbs.Color_2_Green, gbs.Color_2_Blue);
                InitializeImage();
            }
        }

        private void color3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color3RedBox.SelectedIndex != -1 && color3GreenBox.SelectedIndex != -1 && color3BlueBox.SelectedIndex != -1)
            {
                gbs.Color_3_Red = color3RedBox.SelectedIndex * 8;
                gbs.Color_3_Green = color3GreenBox.SelectedIndex * 8;
                gbs.Color_3_Blue = color3BlueBox.SelectedIndex * 8;
                color3Panel.BackColor = Color.FromArgb(gbs.Color_3_Red, gbs.Color_3_Green, gbs.Color_3_Blue);
                InitializeImage();
            }
        }

        private void color4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color4RedBox.SelectedIndex != -1 && color4GreenBox.SelectedIndex != -1 && color4BlueBox.SelectedIndex != -1)
            {
                gbs.Color_4_Red = color4RedBox.SelectedIndex * 8;
                gbs.Color_4_Green = color4GreenBox.SelectedIndex * 8;
                gbs.Color_4_Blue = color4BlueBox.SelectedIndex * 8;
                color4Panel.BackColor = Color.FromArgb(gbs.Color_4_Red, gbs.Color_4_Green, gbs.Color_4_Blue);
                InitializeImage();
            }
        }

        #endregion Changing color based on box values
    }
}