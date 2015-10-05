namespace GBRead.Base
{
    public class GBPalette
    {
        public int Color_1_Red { get { return (Col_1 & 0x1F) << 3; } set { Col_1 = (Col_1 & 0x7FE0) | ((value >> 3) & 0x1F); } }

        public int Color_1_Green { get { return ((Col_1 >> 5) & 0x1F) << 3; } set { Col_1 = (Col_1 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }

        public int Color_1_Blue { get { return ((Col_1 >> 10) & 0x1F) << 3; } set { Col_1 = (Col_1 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }

        public int Color_2_Red { get { return (Col_2 & 0x1F) << 3; } set { Col_2 = (Col_2 & 0x7FE0) | ((value >> 3) & 0x1F); } }

        public int Color_2_Green { get { return ((Col_2 >> 5) & 0x1F) << 3; } set { Col_2 = (Col_2 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }

        public int Color_2_Blue { get { return ((Col_2 >> 10) & 0x1F) << 3; } set { Col_2 = (Col_2 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }

        public int Color_3_Red { get { return (Col_3 & 0x1F) << 3; } set { Col_3 = (Col_3 & 0x7FE0) | ((value >> 3) & 0x1F); } }

        public int Color_3_Green { get { return ((Col_3 >> 5) & 0x1F) << 3; } set { Col_3 = (Col_3 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }

        public int Color_3_Blue { get { return ((Col_3 >> 10) & 0x1F) << 3; } set { Col_3 = (Col_3 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }

        public int Color_4_Red { get { return (Col_4 & 0x1F) << 3; } set { Col_4 = (Col_4 & 0x7FE0) | ((value >> 3) & 0x1F); } }

        public int Color_4_Green { get { return ((Col_4 >> 5) & 0x1F) << 3; } set { Col_4 = (Col_4 & 0x7C1F) | (((value >> 3) & 0x1F) << 5); } }

        public int Color_4_Blue { get { return ((Col_4 >> 10) & 0x1F) << 3; } set { Col_4 = (Col_4 & 0x3FF) | (((value >> 3) & 0x1F) << 10); } }

        public int Col_1 = 0x7FFF;
        public int Col_2 = 0x5AD6;
        public int Col_3 = 0x2D6B;
        public int Col_4 = 0;
    }
}