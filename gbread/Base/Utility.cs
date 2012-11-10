namespace GBRead.Base
{
    using System;
    using System.Text.RegularExpressions;

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
    }
}