using System;
using System.Text.RegularExpressions;

namespace GBRead.Base
{
    public class Utility
    {
        public static bool IsWord(string check)
        {
            return new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$").IsMatch(check);
        }

        public static int GetRealAddress(int bank, int address)
        {
            if (address < 0x4000 || bank == 0)
            {
                return address;
            }
            return ((byte)bank * 0x4000) + ((ushort)address - 0x4000);
        }

        public static int GetPCAddress(int address)
        {
            if (address < 0x4000)
            {
                return address;
            }
            else
            {
                return (address & 0x3FFF) | 0x4000;
            }
        }

        public static int GetBankOfRealAddress(int address)
        {
            return address >> 14;
        }

        public static bool OffsetStringToInt(string text, out int offset)
        {
            offset = int.MinValue;
            string[] parsed = text.Split(new[] { ':' }, 2);
            if (parsed.Length < 2)
            {
                return Utility.StringToInt("$" + text, out offset);
            }
            else
            {
                var bank = 0;
                if (!Utility.StringToInt("0x" + parsed[0], out bank))
                {
                    return false;
                }
                if (!Utility.StringToInt("0x" + parsed[1], out offset))
                {
                    return false;
                }
                if (bank < 0 || offset < 0 || offset >= 0x8000)
                {
                    return false;
                }
                offset = GetRealAddress(bank, offset);
                return true;
            }
        }

        public static bool StringToInt(string check, out int result)
        {
            result = 0;
            var temp = 0L;
            if (!StringToLong(check, out temp))
            {
                return false;
            }
            result = (int)temp;
            return true;
        }

        public static bool StringToLong(string check, out long result)
        {
            check = check.ToLower();
            result = 0;
            if (check.StartsWith("$"))
            {
                check = check.Substring(1);
                try
                {
                    result = Convert.ToInt64(check, 16);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (check.StartsWith("%"))
            {
                check = check.Substring(1);
                try
                {
                    result = Convert.ToInt64(check, 2);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (check.StartsWith("&"))
            {
                check = check.Substring(1);
                try
                {
                    result = Convert.ToInt64(check, 8);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (check.StartsWith("0x"))
            {
                try
                {
                    result = Convert.ToInt64(check, 16);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (check.StartsWith("0b"))
            {
                check = check.Substring(2);
                try
                {
                    result = Convert.ToInt64(check, 2);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else if (check.StartsWith("0o"))
            {
                check = check.Substring(2);
                try
                {
                    result = Convert.ToInt64(check, 8);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                try
                {
                    result = Convert.ToInt64(check);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool GameboyFormatStringToWord(string check, string formatString, out long result)
        {
            result = 0;
            if (check.Length != 9)
            {
                return false;
            }
            var checkIndex = 0;
            ushort finalWord = 0;
            for (int i = 1; i < check.Length; i++)
            {
                if ((checkIndex = formatString.IndexOf(check[i])) == -1)
                {
                    return false;
                }
                finalWord |= (ushort)((checkIndex & 1) << (8 - i));
                finalWord |= (ushort)(((checkIndex >> 1) & 1) << (16 - i));
            }
            result = finalWord;
            return true;
        }

        public static string NumToGameboyFormatString(ulong input, string formatString)
        {
            var returned = "";
            var plane0 = (byte)(input);
            var plane1 = (byte)(input >> 8);

            // Optional Range: ".+#@";
            // Ramp from 0 to 10: " .:-=+*#%@"
            for (int i = 7; i >= 0; i--)
            {
                returned += formatString[((plane0 >> i) & 1) + (((plane1 >> i) & 1) << 1)];
            }
            return returned;
        }

        private static string NumToBaseString(ulong input, string fmt)
        {
            if (input == 0)
            {
                return "0";
            }
            string retInv = "";
            string ret = "";
            var inp = input;
            var fmtLen = (ulong)fmt.Length;
            while (inp != 0)
            {
                retInv += fmt[(int)((inp % fmtLen) - 1)];
                inp /= fmtLen;
            }
            for (int i = retInv.Length; i >= 0; i--)
            {
                ret += retInv[i];
            }
            return ret;
        }

        public static string NumToBinaryString(ulong input)
        {
            return NumToBaseString(input, "01");
        }

        public static string NumToHexString(ulong input)
        {
            return NumToBaseString(input, "0123456789ABCDEF");
        }
    }
}