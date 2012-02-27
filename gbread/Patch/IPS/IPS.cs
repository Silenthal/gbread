using System.Collections.Generic;

namespace GBRead
{
    class IPS
    {
        struct IPSGroup
        {
            public int off;
            public int len;
            public bool isRLE;
            public IPSGroup(int offset, int length, bool rle)
            {
                off = offset;
                len = length;
                isRLE = rle;
            }
        };

        struct IPSCombine
        {
            public int index1;
            public int index2;
            public IPSCombine(int i1 = 0, int i2 = 0)
            {
                index1 = i1;
                index2 = i2;
            }
        };

        public static byte[] PatchNoRLE(ref byte[] originalFile, ref byte[] alteredFile)
        {
            if (originalFile.Length != alteredFile.Length) return null;
            List<IPSGroup> patchGroup = new List<IPSGroup>();
            int fSize = 0;
            for (int i = 0; i < originalFile.Length; )
            {
                while (originalFile[i] == alteredFile[i])
                {
                    i++;
                }
                int len = 1;
                int off = i;
                i++;
                while (originalFile[i] != alteredFile[i])
                {
                    len++;
                    i++;
                    if (i >= originalFile.Length || len >= 0xFFFF) break;
                }
                fSize += 5 + len;
                patchGroup.Add(new IPSGroup(off, len, false));
            }
            byte[] patchFile = new byte[8 + fSize];
            patchFile[0] = (byte)'P';
            patchFile[1] = (byte)'A';
            patchFile[2] = (byte)'T';
            patchFile[3] = (byte)'C';
            patchFile[4] = (byte)'H';
            int curOff = 5;
            foreach (IPSGroup p in patchGroup)
            {
                patchFile[curOff++] = (byte)(p.off >> 16);
                patchFile[curOff++] = (byte)(p.off >> 8);
                patchFile[curOff++] = (byte)p.off;
                patchFile[curOff++] = (byte)(p.len >> 8);
                patchFile[curOff++] = (byte)p.len;
                for (int i = p.off; i < p.off + p.len; i++)
                {
                    patchFile[curOff++] = alteredFile[i];
                }
            }
            patchFile[patchFile.Length - 3] = (byte)'E';
            patchFile[patchFile.Length - 2] = (byte)'O';
            patchFile[patchFile.Length - 1] = (byte)'F';
            return patchFile;
        }

        /*
         * Notes on combining parts, for optimal patch size:
         * Each of the conditions below will result in a smaller regular
         * combine size, rather than the two separate ones. Each one must be
         * greater than the difference between the offset of the two
         * different patch groups. RLS is >= 4:
         * Assuming these conditions:
         * 
         * Regular
         *  -Head (5)
         *  -Length ()
         *  -Off()
         *  
         * RLE
         *  -Head(5)
         *  -RLHead(2)
         *  -RLHeadSize
         *  -RLData(1)
         *  -Off()
         * 
         * Reg->Reg
         * 5 + 1.len
         * Reg->RLE
         * 8 + 1.len - 2.RLS
         * RLE->Reg
         * 8
         * RLE->RLE
         * 11 - 2.RLS
         */
        /*
         * For PatchRLE:
         * TO decide which groups to combine:
         * -Place distances into a priority queue.
         * -Take out the biggest ones, and combine them in one op, placing it back \
         * into the queue.
         */
        /**
        static byte[] Patch(ref byte[] originalFile, ref byte[] alteredFile)
        {
            if (originalFile.Length != alteredFile.Length) return null;
            List<IPSGroup> patchGroup = new List<IPSGroup>();
            for (int i = 0; i < originalFile.Length; )
            {
                if (originalFile[i] == alteredFile[i])
                {
                    i++;
                }
                else
                {
                    int len = 1;
                    int off = i;
                    i++;
                    while (originalFile[i] != alteredFile[i])
                    {
                        len++;
                        i++;
                    }
                    patchGroup.Add(new IPSGroup(off, len, false));
                }
            }
            for (int i = 0; i < patchGroup.Count; i++)
            {

            }
        }
        */
    }
}
