using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using GBRead.Base;

namespace GBRead
{
    public partial class ImageDisplayForm : Form
    {
        BinFile dxtFile;
        List<Bitmap> bitList = new List<Bitmap>();
        public DataLabel dataSect = null;

        public ImageDisplayForm(BinFile dt, DataLabel ds)
        {
            InitializeComponent();
            sizeLabel.Text = String.Format("Image Preview Size: {0} x {0}, 0x{1:X} bytes", sizeBar.Value, 0x10 * sizeBar.Value * sizeBar.Value);
            scaleLabel.Text = String.Format("Image Preview Scale: {0}x", scaleBar.Value);
            dxtFile = dt;
            dataSect = new DataLabel(ds);
            setColors();
            renderImage();
            displayImage();
        }

        #region Rendering images
        private void setColors()
        {
            color1RedBox.SelectedIndex = dataSect.DataPalette.Color_1_Red / 8;
            color1GreenBox.SelectedIndex = dataSect.DataPalette.Color_1_Green / 8;
            color1BlueBox.SelectedIndex = dataSect.DataPalette.Color_1_Blue / 8;
            color2RedBox.SelectedIndex = dataSect.DataPalette.Color_2_Red / 8;
            color2GreenBox.SelectedIndex = dataSect.DataPalette.Color_2_Green / 8;
            color2BlueBox.SelectedIndex = dataSect.DataPalette.Color_2_Blue / 8;
            color3RedBox.SelectedIndex = dataSect.DataPalette.Color_3_Red / 8;
            color3GreenBox.SelectedIndex = dataSect.DataPalette.Color_3_Red / 8;
            color3BlueBox.SelectedIndex = dataSect.DataPalette.Color_3_Red / 8;
            color4RedBox.SelectedIndex = dataSect.DataPalette.Color_4_Red / 8;
            color4GreenBox.SelectedIndex = dataSect.DataPalette.Color_4_Green / 8;
            color4BlueBox.SelectedIndex = dataSect.DataPalette.Color_4_Blue / 8;
            color1Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_1_Red, dataSect.DataPalette.Color_1_Green, dataSect.DataPalette.Color_1_Blue);
            color2Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_2_Red, dataSect.DataPalette.Color_2_Green, dataSect.DataPalette.Color_2_Blue);
            color3Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_3_Red, dataSect.DataPalette.Color_3_Green, dataSect.DataPalette.Color_3_Blue);
            color4Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_4_Red, dataSect.DataPalette.Color_4_Green, dataSect.DataPalette.Color_4_Blue);
        }

        private void renderImage()
        {
            bitList.Clear();
            Color[] palette = new Color[4]
            {
                Color.FromArgb(dataSect.DataPalette.Color_1_Red, dataSect.DataPalette.Color_1_Green, dataSect.DataPalette.Color_1_Blue), 
                Color.FromArgb(dataSect.DataPalette.Color_2_Red, dataSect.DataPalette.Color_2_Green, dataSect.DataPalette.Color_2_Blue), 
                Color.FromArgb(dataSect.DataPalette.Color_3_Red, dataSect.DataPalette.Color_3_Green, dataSect.DataPalette.Color_3_Blue), 
                Color.FromArgb(dataSect.DataPalette.Color_4_Red, dataSect.DataPalette.Color_4_Green, dataSect.DataPalette.Color_4_Blue)
            };
            for (int i = dataSect.Offset; i < dataSect.Offset + dataSect.Length; i += 0x10)
            {
                Bitmap ft = new Bitmap(8, 8);
                for (int ix = 0; ix < 8; ix++)
                {
                    int colorLow = dxtFile.ReadByte(i + (2 * ix));
                    int colorHigh = dxtFile.ReadByte(i + (2 * ix) + 1);
                    int color1 = (((colorHigh >> 7) & 1) << 1) | ((colorLow >> 7) & 1);
                    int color2 = (((colorHigh >> 6) & 1) << 1) | ((colorLow >> 6) & 1);
                    int color3 = (((colorHigh >> 5) & 1) << 1) | ((colorLow >> 5) & 1);
                    int color4 = (((colorHigh >> 4) & 1) << 1) | ((colorLow >> 4) & 1);
                    int color5 = (((colorHigh >> 3) & 1) << 1) | ((colorLow >> 3) & 1);
                    int color6 = (((colorHigh >> 2) & 1) << 1) | ((colorLow >> 2) & 1);
                    int color7 = (((colorHigh >> 1) & 1) << 1) | ((colorLow >> 1) & 1);
                    int color8 = ((colorHigh & 1) << 1) | (colorLow & 1);
                    ft.SetPixel(0, ix, palette[color1]);
                    ft.SetPixel(1, ix, palette[color2]);
                    ft.SetPixel(2, ix, palette[color3]);
                    ft.SetPixel(3, ix, palette[color4]);
                    ft.SetPixel(4, ix, palette[color5]);
                    ft.SetPixel(5, ix, palette[color6]);
                    ft.SetPixel(6, ix, palette[color7]);
                    ft.SetPixel(7, ix, palette[color8]);
                }
                bitList.Add(ft);
            }
        }

        private void displayImage()
        {
            int width = sizeBar.Value;
            Bitmap drawnX = new Bitmap(width * 8, width * 8);
            System.Drawing.Graphics gx = System.Drawing.Graphics.FromImage(drawnX);
            gx.Clear(Color.DeepPink);
            for (int i = 0; i < width * width; i++)
            {
                int hOff = (i % width) * 8;
                int vOff = ((i - (i % width)) / width) * 8;
                if (i < bitList.Count)
                {
                    gx.DrawImageUnscaled(bitList[i], hOff, vOff);
                }
            }
            if (scaleBar.Value > 1)
            {
                Bitmap scaledft = new Bitmap(drawnX.Width * scaleBar.Value, drawnX.Height * scaleBar.Value);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(scaledft);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(drawnX, 0, 0, scaledft.Width, scaledft.Height);
                pictureBox1.Image = scaledft;
            }
            else pictureBox1.Image = drawnX;
            sizeLabel.Text = String.Format("Image Preview Size: {0} x {0}, 0x{1:X} bytes", sizeBar.Value, 0x10 * sizeBar.Value * sizeBar.Value);
        }
        #endregion

        #region Scaling options
        private void sizeBar_Scroll(object sender, System.EventArgs e)
        {
            displayImage();
            sizeLabel.Text = String.Format("Image Preview Size: {0} x {0}, 0x{1:X} bytes", sizeBar.Value, 0x10 * sizeBar.Value * sizeBar.Value);
        }

        private void scaleBar_Scroll(object sender, System.EventArgs e)
        {
            displayImage();
            scaleLabel.Text = String.Format("Image Preview Scale: {0}x", scaleBar.Value);
        }
        #endregion

        #region Changing color based on box values
        private void color1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color1RedBox.SelectedIndex != -1 && color1GreenBox.SelectedIndex != -1 && color1BlueBox.SelectedIndex != -1)
            {
                dataSect.DataPalette.Color_1_Red = color1RedBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_1_Green = color1GreenBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_1_Blue = color1BlueBox.SelectedIndex * 8;
                color1Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_1_Red, dataSect.DataPalette.Color_1_Green, dataSect.DataPalette.Color_1_Blue);
                renderImage();
                displayImage();
            }
        }

        private void color2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color2RedBox.SelectedIndex != -1 && color2GreenBox.SelectedIndex != -1 && color2BlueBox.SelectedIndex != -1)
            {
                dataSect.DataPalette.Color_2_Red = color2RedBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_2_Green = color2GreenBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_2_Blue = color2BlueBox.SelectedIndex * 8;
                color2Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_2_Red, dataSect.DataPalette.Color_2_Green, dataSect.DataPalette.Color_2_Blue);
                renderImage();
                displayImage();
            }
        }

        private void color3_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color3RedBox.SelectedIndex != -1 && color3GreenBox.SelectedIndex != -1 && color3BlueBox.SelectedIndex != -1)
            {
                dataSect.DataPalette.Color_3_Red = color3RedBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_3_Green = color3GreenBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_3_Blue = color3BlueBox.SelectedIndex * 8;
                color3Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_3_Red, dataSect.DataPalette.Color_3_Green, dataSect.DataPalette.Color_3_Blue);
                renderImage();
                displayImage();
            }
        }

        private void color4_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (color4RedBox.SelectedIndex != -1 && color4GreenBox.SelectedIndex != -1 && color4BlueBox.SelectedIndex != -1)
            {
                dataSect.DataPalette.Color_4_Red = color4RedBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_4_Green = color4GreenBox.SelectedIndex * 8;
                dataSect.DataPalette.Color_4_Blue = color4BlueBox.SelectedIndex * 8;
                color4Panel.BackColor = Color.FromArgb(dataSect.DataPalette.Color_4_Red, dataSect.DataPalette.Color_4_Green, dataSect.DataPalette.Color_4_Blue);
                renderImage();
                displayImage();
            }
        }
        #endregion
    }
}
