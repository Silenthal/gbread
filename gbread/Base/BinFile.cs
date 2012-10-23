using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GBRead.Base
{
    public abstract class BinFile
    {
        protected byte[] binFile;

        public byte[] MainFile
        {
            get
            {
                return binFile;
            }
        }

        public bool FileLoaded { get { return (binFile != null && binFile.Length > 0); } }

        public int Length { get { return (binFile != null ? binFile.Length : 0); } }

        public BinFile() { }

        public BinFile(byte[] rFile)
        {
            LoadFile(rFile);
        }

        public void LoadFile(string fileName)
        {
            FileInfo fs = new FileInfo(fileName);
            try
            {
                binFile = File.ReadAllBytes(fileName);
            }
            catch (FileNotFoundException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public void LoadFile(byte[] rFile)
        {
            if (rFile == null || rFile.Length == 0) return;
            binFile = new byte[rFile.Length];
            Array.Copy(rFile, binFile, binFile.Length);
        }

        public bool ModifyFile(int offset, byte[] insertFile)
        {
            if (!FileLoaded || insertFile == null || offset < 0 || insertFile.Length == 0 || offset + insertFile.Length >= binFile.Length)
            {
                return false;
            }
            else
            {
                Array.Copy(insertFile, 0, binFile, offset, insertFile.Length);
                return true;
            }
        }

        public bool ModifyFile(int offset, BinFile insertFile)
        {
            return ModifyFile(offset, insertFile.binFile);
        }

        public bool SaveFile(string newFileName)
        {
            if (!FileLoaded) return false;
            if (binFile.Length > 0)
            {
                File.WriteAllBytes(newFileName, binFile);
                return true;
            }
            else return false;
        }

        public bool SaveFilePortion(string fileName, int offset, int length)
        {
            if (!FileLoaded) return false;
            if (offset + length >= binFile.Length) return false;
            using (BinaryWriter bs = new BinaryWriter(File.OpenWrite(fileName)))
            {
                bs.Write(binFile, offset, length);
            }
            return true;
        }

        public byte ReadByte(int offset)
        {
            if (binFile == null || offset < 0 || offset >= binFile.Length)
            {
                return 0xFF;
            }
            return binFile[offset];
        }

        public ushort ReadWord(int offset)
        {
            if (binFile == null || offset < 0 || offset > binFile.Length - 2)
            {
                return 0xFFFF;
            }
            ushort ret = binFile[offset + 1];
            ret <<= 8;
            ret |= binFile[offset];
            return ret;
        }

        public abstract string GetBinInfo();
    }
}