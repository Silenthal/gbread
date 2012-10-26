namespace GBRead.Base
{
    using System;
    /// <summary>
    /// Contains utility methods used elsewhere in the program.
    /// </summary>
    public class Utility
    {
        public static int GetRealAddress(byte bank, ushort address)
        {
            if (address < 0x4000 || bank == 0)
            {
                return address;
            }
            return (bank * 0x4000) + (address - 0x4000);
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

        public static int NumStringToInt(string check)
        {
            check = check.ToLower();
            if (check.StartsWith("0x"))
            {
                return Convert.ToInt32(check, 16);
            }
            else if (check.StartsWith("$"))
            {
                check = check.Substring(1);
                return Convert.ToInt32(check, 16);
            }
            else if (check.StartsWith("0o"))
            {
                check = check.Substring(2);
                return Convert.ToInt32(check, 8);
            }
            else if (check.StartsWith("&"))
            {
                check = check.Substring(1);
                return Convert.ToInt32(check, 8);
            }
            else if (check.StartsWith("0b"))
            {
                check = check.Substring(2);
                return Convert.ToInt32(check, 2);
            }
            else if (check.StartsWith("%"))
            {
                check = check.Substring(1);
                return Convert.ToInt32(check, 2);
            }
            else
            {
                return Convert.ToInt32(check);
            }
        }
    }
}
