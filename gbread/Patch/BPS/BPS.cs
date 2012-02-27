using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections;
using System.Linq;
using System.Text;

namespace GBRead
{
    public class BPS
    {
        public static List<byte> Encode(UInt64 data)
        {
            List<byte> rtf = new List<byte>();
            while (true)
            {
                UInt64 x = data & 0x7f;
                data >>= 7;
                if (data == 0)
                {
                    rtf.Add((byte)(0x80 | x));
                    break;
                }
                rtf.Add((byte)x);
                data--;
            }
            return rtf;
        }

        public void Write(BinaryWriter bw, UInt64 data)
        {
            bw.Write(data);
        }

        public static List<byte> Encode(byte[] data)
        {
            UInt64 asdf = 0xffffffffffffffff;
            List<byte> rtf = new List<byte>();
            int shAmt = 1;
            foreach (byte b in data)
            {
            }
            return null;
        }

        public static byte[] WriteFile(byte[] originalFile, byte[] modifiedFile, byte[] metaData)
        {
            List<byte> patchFile = new List<byte>()
            {
                (byte)'B', 
                (byte)'P', 
                (byte)'S', 
                (byte)'1'
            };
            patchFile.AddRange(Encode((UInt64)originalFile.LongLength));
            patchFile.AddRange(Encode((UInt64)modifiedFile.LongLength));
            
            byte[] returned = new byte[patchFile.Count];
            for (int i = 0; i < patchFile.Count; i++)
            {
                returned[i] = patchFile[i];
            }
            return returned;
        }

        public static byte[] CreateLinearPatch(byte[] originalFile, byte[] modifyFile, string metadataFileName)
        {
            return null;
        }
    }
}
