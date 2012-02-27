using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace GBRead.Base
{
	class GBImage
	{

		public GBImage()
		{

		}

		public static List<Bitmap> renderImageTiles(BinFile dxtFile, int offset, int length, Color[] palette)
		{
			List<Bitmap> tileList = new List<Bitmap>();
			for (int i = offset; i < offset + length; i += 0x10)
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
				tileList.Add(ft);
			}
			return tileList;
		}

		public static Image ConstructImageFromTileList(int TileXCount, int TileYCount, int scale, List<Bitmap> tileList)
		{
			Bitmap drawnX = new Bitmap(TileXCount * 8, TileYCount * 8);
			System.Drawing.Graphics gx = System.Drawing.Graphics.FromImage(drawnX);
			gx.Clear(Color.DeepPink);
			for (int i = 0; i < TileXCount * TileYCount; i++)
			{
				int hOff = (i % TileXCount) * 8;
				int vOff = ((i - (i % TileXCount)) / TileXCount) * 8;
				if (i < tileList.Count)
				{
					gx.DrawImageUnscaled(tileList[i], hOff, vOff);
				}
			}
			if (scale > 1)
			{
				Bitmap scaledft = new Bitmap(drawnX.Width * scale, drawnX.Height * scale);
				System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(scaledft);
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage(drawnX, 0, 0, scaledft.Width, scaledft.Height);
				return scaledft;
			}
			else return drawnX;
		}
	}
}
