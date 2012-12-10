using System;
using System.IO;
using System.Text;

namespace GBRead.Base
{
    public class BinFile
    {
        private byte[] binFile;

        public byte[] MainFile
        {
            get
            {
                return binFile;
            }
        }

        #region Offsets of information in header

        private static int logoOffset = 0x104;
        private static int titleOffset = 0x134;
        private static int gbcManufacturerCode = 0x13F;
        private static int cartTypeOffset = 0x143;
        private static int newLicenseCodeOff_1 = 0x144;
        private static int newLicenseCodeOff_2 = 0x145;
        private static int superGBSupportOffset = 0x146;
        private static int romTypeOffset = 0x147;
        private static int romSizeOffset = 0x148;
        private static int ramSizeOffset = 0x149;
        private static int countryCodeOffset = 0x14A;
        private static int oldLicenseCode = 0x14B;
        private static int versionOffset = 0x14C;
        private static int complementOffset = 0x14D;
        private static int checksumOffHi = 0x14E;
        private static int checksumOffLo = 0x14F;

        #endregion Offsets of information in header

        private static byte[] nintendoLogo = new byte[]
		{
			0xCE, 0xED, 0x66, 0x66, 0xCC, 0x0D, 0x00, 0x0B, 0x03, 0x73, 0x00, 0x83, 0x00, 0x0C, 0x00, 0x0D,
			0x00, 0x08, 0x11, 0x1F, 0x88, 0x89, 0x00, 0x0E, 0xDC, 0xCC, 0x6E, 0xE6, 0xDD, 0xDD, 0xD9, 0x99,
			0xBB, 0xBB, 0x67, 0x63, 0x6E, 0x0E, 0xEC, 0xCC, 0xDD, 0xDC, 0x99, 0x9F, 0xBB, 0xB9, 0x33, 0x3E
		};

        public BinFile()
        {
        }

        public BinFile(byte[] rFile)
        {
            LoadFile(rFile);
        }

        public bool FileLoaded
        {
            get
            {
                return (binFile != null && binFile.Length > 0);
            }
        }

        public int Length
        {
            get
            {
                return (binFile != null ? binFile.Length : 0);
            }
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
            if (rFile == null || rFile.Length == 0)
                return;
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
            if (!FileLoaded)
                return false;
            if (binFile.Length > 0)
            {
                File.WriteAllBytes(newFileName, binFile);
                return true;
            }
            else
                return false;
        }

        public bool SaveFilePortion(string fileName, int offset, int length)
        {
            if (!FileLoaded)
                return false;
            if (offset + length >= binFile.Length)
                return false;
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

        public uint ReadDWord(int offset)
        {
            if (binFile == null || offset < 0 || offset > binFile.Length - 2)
            {
                return 0xFFFFFFFF;
            }
            uint ret = binFile[offset + 4];
            ret <<= 8;
            ret |= binFile[offset + 3];
            ret <<= 8;
            ret |= binFile[offset + 2];
            ret <<= 8;
            ret |= binFile[offset];
            return ret;
        }

        public ulong ReadQWord(int offset)
        {
            if (binFile == null || offset < 0 || offset > binFile.Length - 2)
            {
                return 0xFFFFFFFFFFFFFFFF;
            }
            ulong ret = binFile[offset + 7];
            ret <<= 8;
            ret |= binFile[offset + 6];
            ret <<= 8;
            ret |= binFile[offset + 5];
            ret <<= 8;
            ret |= binFile[offset + 4];
            ret <<= 8;
            ret |= binFile[offset + 3];
            ret <<= 8;
            ret |= binFile[offset + 2];
            ret <<= 8;
            ret |= binFile[offset + 1];
            ret <<= 8;
            ret |= binFile[offset];
            return ret;
        }

        public string GetBinInfo()
        {
            if (binFile.Length < 0x150)
                return "Not a properly headered GB/C file.";
            string mCode = GetManufacturerCode();
            StringBuilder info = new StringBuilder();
            info.AppendLine("Logo Check: " + (IsLogoGood() ? "Passed" : "Failed"));
            info.AppendLine("Rom Title: " + GetTitle());
            if (mCode.ToString() != String.Empty)
                info.AppendLine("Manufacturer Code: " + mCode.ToString());
            info.AppendLine("Cartridge Type: " + GetCompatibility());
            info.AppendLine("Licensee Code: " + GetLicenseCode());
            info.AppendLine("Super GB Functions: " + (ReadByte(superGBSupportOffset) == 0x03 ? "Yes" : "No"));
            info.AppendLine("Reported ROM/Memory Bank Controller Type: " + GetStorageInfo());
            info.AppendLine("Reported ROM Size: " + GetROMSize());
            info.AppendLine("Reported RAM Size: " + GetRAMSize());
            info.AppendLine("Country Code: " + (ReadByte(countryCodeOffset) == 0 ? "Japanese" : "Non-Japanese"));
            info.AppendLine(String.Format("Version Number: {0:X2}", ReadByte(versionOffset)));
            info.AppendLine(String.Format("Header Complement Check: {0:X2} | Actual: {1:X2}", ReadByte(complementOffset), GBComplementCheck()));
            info.AppendLine(String.Format("ROM Checksum: {0:X2}{1:X2} | Actual: {2:X}", ReadByte(checksumOffHi), ReadByte(checksumOffLo), GBCheckSum()));
            return info.ToString();
        }

        private int GBCheckSum()
        {
            if (!FileLoaded || binFile.Length < 0x150)
                return Int32.MinValue;
            ushort checksum = 0;
            foreach (Byte bt in binFile)
            {
                checksum += bt;
            }
            checksum -= binFile[complementOffset];
            checksum -= binFile[checksumOffHi];
            checksum -= binFile[checksumOffLo];
            checksum += (byte)GBComplementCheck();
            return checksum;
        }

        private int GBComplementCheck()
        {
            if (!FileLoaded || binFile.Length < 0x150)
                return Int32.MinValue;
            byte compCheck = 0;
            for (int i = titleOffset; i < complementOffset; i++)
            {
                byte x = binFile[i];
                compCheck -= ++x;
            }
            return compCheck;
        }

        public void ValidateFile()
        {
        }

        private string GetTitle()
        {
            string title = "";
            for (int i = 0; i < (ReadByte(cartTypeOffset) == 0 ? 15 : 11); i++)
            {
                title += ReadByte(titleOffset + i) == 0 ? ' ' : (char)ReadByte(titleOffset + i);
            }
            return title;
        }

        private bool IsLogoGood()
        {
            for (int i = 0; i < nintendoLogo.Length; i++)
            {
                if (ReadByte(logoOffset + i) != nintendoLogo[i])
                {
                    return false;
                }
            }
            return true;
        }

        private string GetManufacturerCode()
        {
            if (ReadByte(cartTypeOffset) == 0x80 || ReadByte(cartTypeOffset) == 0xC0)
            {
                string mCode = "";
                for (int i = 0; i < 4; i++)
                {
                    mCode += ReadByte(gbcManufacturerCode + i) == 0 ? ' ' : (char)ReadByte(gbcManufacturerCode + i);
                }
                return mCode;
            }
            else
                return "";
        }

        private string GetCompatibility()
        {
            switch (ReadByte(cartTypeOffset))
            {
                case 0x0:
                    return "Game Boy";
                case 0x80:
                    return "Game Boy Color, Game Boy Compatible";
                case 0x84:
                    return "Colorized Game Boy";
                case 0x88:
                    return "Colorized Game Boy";
                case 0xC0:
                    return "Game Boy Color Only";
                default:
                    return "Unrecognized";
            }
        }

        private string GetLicenseCode()
        {
            if (ReadByte(oldLicenseCode) == 0x33)
            {
                string licenseCode = "";
                licenseCode += ReadByte(newLicenseCodeOff_1) == 0 ? ' ' : (char)ReadByte(newLicenseCodeOff_1);
                licenseCode += ReadByte(newLicenseCodeOff_2) == 0 ? ' ' : (char)ReadByte(newLicenseCodeOff_2);
                return licenseCode;
            }
            else
                return ReadByte(oldLicenseCode).ToString("X2");
        }

        private string GetStorageInfo()
        {
            switch (ReadByte(romTypeOffset))
            {
                case 0x00:
                    return "ROM Only";
                case 0x1:
                    return "MBC1";
                case 0x2:
                    return "MBC1 + RAM";
                case 0x3:
                    return "MBC1 + RAM + Battery";
                case 0x5:
                    return "MBC2";
                case 0x6:
                    return "MBC2 + Battery";
                case 0x8:
                    return "ROM + RAM";
                case 0x9:
                    return "ROM + RAM + Battery";
                case 0xB:
                    return "MMM01";
                case 0xC:
                    return "MMM01 + RAM";
                case 0xD:
                    return "MMM01 + RAM + Battery";
                case 0xF:
                    return "MBC3 + Timer + Battery";
                case 0x10:
                    return "MBC3 + Timer + RAM + Battery";
                case 0x11:
                    return "MBC3";
                case 0x12:
                    return "MBC3 + RAM";
                case 0x13:
                    return "MBC3 + RAM + Battery";
                case 0x15:
                    return "MBC4";
                case 0x16:
                    return "MBC4 + RAM";
                case 0x17:
                    return "MBC4 + RAM + Battery";
                case 0x19:
                    return "MBC5";
                case 0x1A:
                    return "MBC5 + RAM";
                case 0x1B:
                    return "MBC5 + RAM + Battery";
                case 0x1C:
                    return "MBC5 + Rumble";
                case 0x1D:
                    return "MBC5 + Rumble + RAM";
                case 0x1E:
                    return "MBC5 + Rumble + RAM + Battery";
                case 0xFC:
                    return "Pocket Camera";
                case 0xFD:
                    return "Bandai TAMA5";
                case 0xFE:
                    return "HuC3";
                case 0xFF:
                    return "HuC1 + RAM + Battery";
                default:
                    return "Unrecognized";
            }
        }

        private string GetROMSize()
        {
            byte size = (byte)ReadByte(romSizeOffset);
            int sizeInKBit = size <= 7 ? (256 << size) : (8 + (1 << (size - 0x52)) * 1024);
            int sizeInKByt = sizeInKBit >> 3;
            return String.Format("{0} {1}Bit/{2} {3}B",
                sizeInKBit >= 1024 ? sizeInKBit >> 10 : sizeInKBit,
                sizeInKBit >= 1024 ? "m" : "k",
                sizeInKByt >= 1024 ? sizeInKByt >> 10 : sizeInKByt,
                sizeInKBit >= 1024 ? "m" : "k");
        }

        private string GetRAMSize()
        {
            switch (ReadByte(ramSizeOffset))
            {
                case 0:
                    return "None";
                case 1:
                    return "2 KB";
                case 2:
                    return "8 KB";
                case 3:
                    return "16 KB";
                default:
                    return "Unrecognized value";
            }
        }

        public void FixGBHeader()
        {
            int compCheck = GBComplementCheck();
            int checksum = GBCheckSum();
            if (compCheck != Int32.MinValue && checksum != Int32.MinValue)
            {
                ModifyFile(0x14D, new byte[] { (byte)compCheck });
                ModifyFile(0x14E, new byte[] { (byte)(checksum >> 8), (byte)checksum });
            }
        }
    }
}